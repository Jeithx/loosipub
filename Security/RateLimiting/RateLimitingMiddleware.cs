using Core.Utilities.Security.RateLimiting.Models;
using Core.Utilities.Security.RateLimiting.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Core.Utilities.Security.RateLimiting
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IRateLimitingService _rateLimitingService;
        private readonly ILogger<RateLimitingMiddleware> _logger;
        private readonly RateLimitOptions _options;

        public RateLimitingMiddleware(
            RequestDelegate next,
            IRateLimitingService rateLimitingService,
            ILogger<RateLimitingMiddleware> logger,
            RateLimitOptions options)
        {
            _next = next;
            _rateLimitingService = rateLimitingService;
            _logger = logger;
            _options = options;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var clientIP = GetClientIP(context);
            var endpoint = GetEndpointKey(context);

            // Kara liste kontrolü
            if (IsBlacklisted(clientIP))
            {
                _logger.LogWarning("Kara listeli IP'den erişim engellendi. IP: {ClientIP}, Endpoint: {Endpoint}", clientIP, endpoint);
                await WriteRateLimitResponse(context, HttpStatusCode.Forbidden, "IP address is blacklisted", 0);
                return;
            }

            // Beyaz liste kontrolü - beyaz listede ise rate limit uygulama
            if (IsWhitelisted(clientIP))
            {
                await _next(context);
                return;
            }

            // Static dosyalar ve health check endpoint'leri için rate limit uygulama
            if (ShouldSkipRateLimit(context))
            {
                await _next(context);
                return;
            }

            try
            {
                // Rate limit kontrolü
                var rateLimitResult = await _rateLimitingService.CheckRateLimitAsync(clientIP, endpoint);

                // Rate limit header'larını ekle
                AddRateLimitHeaders(context, rateLimitResult);

                if (!rateLimitResult.IsAllowed)
                {
                    // Rate limit aşıldı
                    _logger.LogWarning("Rate limit aşıldı. IP: {ClientIP}, Endpoint: {Endpoint}, RequestCount: {RequestCount}",
                        clientIP, endpoint, rateLimitResult.RequestCount);

                    await WriteRateLimitResponse(context, HttpStatusCode.TooManyRequests,
                        "Rate limit exceeded. Please try again later.", rateLimitResult.RetryAfterSeconds);
                    return;
                }

                // Request devam edebilir
                await _next(context);

                // Başarılı request sonrası log
                _logger.LogDebug("Request işlendi. IP: {ClientIP}, Endpoint: {Endpoint}, RequestCount: {RequestCount}",
                    clientIP, endpoint, rateLimitResult.RequestCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Rate limiting kontrolü sırasında hata oluştu. IP: {ClientIP}", clientIP);
                // Hata durumunda request'e devam et
                await _next(context);
            }
        }

        private string GetClientIP(HttpContext context)
        {
            // X-Forwarded-For header'ını kontrol et (proxy/load balancer kullanımı için)
            var xForwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(xForwardedFor))
            {
                // İlk IP'yi al (client IP)
                return xForwardedFor.Split(',')[0].Trim();
            }

            // X-Real-IP header'ını kontrol et
            var xRealIp = context.Request.Headers["X-Real-IP"].FirstOrDefault();
            if (!string.IsNullOrEmpty(xRealIp))
            {
                return xRealIp.Trim();
            }

            // CF-Connecting-IP (Cloudflare)
            var cfConnectingIp = context.Request.Headers["CF-Connecting-IP"].FirstOrDefault();
            if (!string.IsNullOrEmpty(cfConnectingIp))
            {
                return cfConnectingIp.Trim();
            }

            // Remote IP Address
            return context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        }

        private string GetEndpointKey(HttpContext context)
        {
            var method = context.Request.Method.ToUpperInvariant();
            var path = context.Request.Path.Value?.ToLowerInvariant() ?? string.Empty;

            // Endpoint'i normalize et (ID'leri kaldır)
            path = NormalizePath(path);

            return $"{method}:{path}";
        }

        private string NormalizePath(string path)
        {
            // Path'teki ID'leri generic hale getir
            // Örnek: /api/products/123 -> /api/products/{id}
            var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < segments.Length; i++)
            {
                // Numeric ID'leri tespit et
                if (int.TryParse(segments[i], out _) || Guid.TryParse(segments[i], out _))
                {
                    segments[i] = "{id}";
                }
            }

            return "/" + string.Join("/", segments);
        }

        private bool IsBlacklisted(string clientIP)
        {
            return _options.BlacklistedIPs.Contains(clientIP);
        }

        private bool IsWhitelisted(string clientIP)
        {
            return _options.WhitelistedIPs.Contains(clientIP);
        }

        private bool ShouldSkipRateLimit(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLowerInvariant();

            if (string.IsNullOrEmpty(path))
                return false;

            // Health check, metrics, static dosyalar için rate limit uygulama
            var skipPaths = new[]
            {
            "/health",
            "/healthcheck",
            "/ping",
            "/metrics",
            "/favicon.ico",
            "/.well-known/"
        };

            return skipPaths.Any(skipPath => path.StartsWith(skipPath));
        }

        private void AddRateLimitHeaders(HttpContext context, RateLimitResult rateLimitResult)
        {
            context.Response.Headers.Add("X-RateLimit-Limit", GetCurrentLimit(GetEndpointKey(context)).ToString());
            context.Response.Headers.Add("X-RateLimit-Remaining",
                Math.Max(0, GetCurrentLimit(GetEndpointKey(context)) - rateLimitResult.RequestCount).ToString());
            context.Response.Headers.Add("X-RateLimit-Reset",
                ((DateTimeOffset)rateLimitResult.NextResetTime).ToUnixTimeSeconds().ToString());

            if (!rateLimitResult.IsAllowed && rateLimitResult.RetryAfterSeconds > 0)
            {
                context.Response.Headers.Add("Retry-After", rateLimitResult.RetryAfterSeconds.ToString());
            }
        }

        private int GetCurrentLimit(string endpoint)
        {
            return endpoint.ToLowerInvariant() switch
            {
                var e when e.Contains("/auth/login") => _options.AuthEndpointLimit,
                var e when e.Contains("/auth/register") => _options.AuthEndpointLimit,
                var e when e.Contains("post:") => _options.WriteEndpointLimit,
                var e when e.Contains("put:") => _options.WriteEndpointLimit,
                var e when e.Contains("delete:") => _options.WriteEndpointLimit,
                _ => _options.GeneralEndpointLimit
            };
        }

        private async Task WriteRateLimitResponse(HttpContext context, HttpStatusCode statusCode, string message, int retryAfterSeconds)
        {
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            if (retryAfterSeconds > 0)
            {
                context.Response.Headers.Add("Retry-After", retryAfterSeconds.ToString());
            }

            var response = new
            {
                error = new
                {
                    code = statusCode.ToString(),
                    message = message,
                    timestamp = DateTime.UtcNow,
                    retryAfter = retryAfterSeconds > 0 ? retryAfterSeconds : 0
                }
            };

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
