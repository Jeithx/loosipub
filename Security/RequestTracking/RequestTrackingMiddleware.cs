using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace Core.Utilities.Security.RequestTracking
{
    public class RequestTrackingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestTrackingMiddleware> _logger;
        private static readonly HashSet<string> SensitiveHeaders = new(StringComparer.OrdinalIgnoreCase)
    {
        "Authorization", "X-API-Key", "Cookie", "Set-Cookie", "X-Auth-Token"
    };

        public RequestTrackingMiddleware(RequestDelegate next, ILogger<RequestTrackingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var requestId = Guid.NewGuid().ToString("N")[..8];

            // Request context'e ID ekle
            context.Items["RequestId"] = requestId;

            // Request bilgilerini topla
            var requestInfo = await CollectRequestInfo(context, requestId);

            // Response bilgilerini yakalamak için stream'i wrap et
            var originalResponseBody = context.Response.Body;
            using var responseBodyStream = new MemoryStream();
            context.Response.Body = responseBodyStream;

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Request işlenirken hata oluştu. RequestId: {RequestId}", requestId);
                throw;
            }
            finally
            {
                stopwatch.Stop();

                // Response bilgilerini topla
                var responseInfo = await CollectResponseInfo(context, responseBodyStream, originalResponseBody);

                // Tüm bilgileri logla
                LogRequestResponse(requestInfo, responseInfo, stopwatch.ElapsedMilliseconds, requestId);
            }
        }

        private async Task<RequestInfo> CollectRequestInfo(HttpContext context, string requestId)
        {
            var request = context.Request;

            // Request body'yi oku (eğer varsa)
            string requestBody = null;
            if (request.ContentLength > 0 && request.Body.CanRead)
            {
                request.EnableBuffering();
                var buffer = new byte[Convert.ToInt32(request.ContentLength)];
                await request.Body.ReadAsync(buffer, 0, buffer.Length);
                requestBody = Encoding.UTF8.GetString(buffer);
                request.Body.Position = 0;
            }

            return new RequestInfo
            {
                RequestId = requestId,
                Method = request.Method,
                Path = request.Path.Value,
                QueryString = request.QueryString.Value,
                Headers = FilterSensitiveHeaders(request.Headers),
                Body = SanitizeRequestBody(requestBody, request.ContentType),
                ClientIP = GetClientIP(context),
                UserAgent = request.Headers["User-Agent"].FirstOrDefault(),
                Timestamp = DateTime.UtcNow,
                UserId = context.User.Identity?.IsAuthenticated == true ?
                         context.User.FindFirst("sub")?.Value ?? context.User.FindFirst("id")?.Value : null,
                ApiKey = GetMaskedApiKey(request.Headers["X-API-Key"].FirstOrDefault())
            };
        }

        private async Task<ResponseInfo> CollectResponseInfo(HttpContext context, MemoryStream responseBodyStream, Stream originalResponseBody)
        {
            var response = context.Response;

            // Response body'yi oku
            responseBodyStream.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(responseBodyStream).ReadToEndAsync();

            // Original stream'e geri yaz
            responseBodyStream.Seek(0, SeekOrigin.Begin);
            await responseBodyStream.CopyToAsync(originalResponseBody);
            context.Response.Body = originalResponseBody;

            return new ResponseInfo
            {
                StatusCode = response.StatusCode,
                Headers = FilterSensitiveHeaders(response.Headers.ToDictionary(h => h.Key, h => h.Value.AsEnumerable())),
                Body = SanitizeResponseBody(responseBody, response.ContentType),
                ContentLength = responseBodyStream.Length
            };
        }

        private void LogRequestResponse(RequestInfo requestInfo, ResponseInfo responseInfo, long elapsedMs, string requestId)
        {
            var logLevel = DetermineLogLevel(responseInfo.StatusCode, elapsedMs);

            var logData = new
            {
                RequestId = requestId,
                Request = requestInfo,
                Response = new
                {
                    responseInfo.StatusCode,
                    responseInfo.ContentLength,
                    Headers = responseInfo.Headers,
                    Body = responseInfo.Body
                },
                Performance = new
                {
                    ElapsedMilliseconds = elapsedMs,
                    IsSlowRequest = elapsedMs > 5000 // 5 saniyeden uzun
                },
                Security = new
                {
                    IsAuthenticated = !string.IsNullOrEmpty(requestInfo.UserId),
                    HasApiKey = !string.IsNullOrEmpty(requestInfo.ApiKey),
                    IsPotentiallyMalicious = DetectSuspiciousActivity(requestInfo, responseInfo)
                }
            };

            _logger.Log(logLevel, "API Request completed: {@LogData}", logData);

            // Güvenlik olaylarını ayrıca logla
            if (logData.Security.IsPotentiallyMalicious)
            {
                _logger.LogWarning("Şüpheli aktivite tespit edildi: {@SecurityEvent}", new
                {
                    RequestId = requestId,
                    ClientIP = requestInfo.ClientIP,
                    UserAgent = requestInfo.UserAgent,
                    Path = requestInfo.Path,
                    StatusCode = responseInfo.StatusCode,
                    Timestamp = requestInfo.Timestamp
                });
            }
        }

        private LogLevel DetermineLogLevel(int statusCode, long elapsedMs)
        {
            if (statusCode >= 500) return LogLevel.Error;
            if (statusCode >= 400) return LogLevel.Warning;
            if (elapsedMs > 5000) return LogLevel.Warning; // Yavaş requestler
            return LogLevel.Information;
        }

        private bool DetectSuspiciousActivity(RequestInfo request, ResponseInfo response)
        {
            // Basit şüpheli aktivite tespiti
            var suspiciousPatterns = new[]
            {
            "script", "alert", "javascript:", "vbscript:", "onload", "onerror",
            "../", "..\\", "/etc/passwd", "cmd.exe", "powershell",
            "union select", "drop table", "insert into", "delete from"
        };

            var contentToCheck = $"{request.Path} {request.QueryString} {request.Body}".ToLowerInvariant();

            return suspiciousPatterns.Any(pattern => contentToCheck.Contains(pattern)) ||
                   response.StatusCode == 401 || // Unauthorized denemeler
                   response.StatusCode == 403;   // Forbidden erişim denemeleri
        }

        private Dictionary<string, IEnumerable<string>> FilterSensitiveHeaders(IHeaderDictionary headers)
        {
            return headers
                .Where(h => !SensitiveHeaders.Contains(h.Key))
                .ToDictionary(h => h.Key, h => h.Value.AsEnumerable());
        }

        private Dictionary<string, IEnumerable<string>> FilterSensitiveHeaders(IDictionary<string, IEnumerable<string>> headers)
        {
            return headers
                .Where(h => !SensitiveHeaders.Contains(h.Key))
                .ToDictionary(h => h.Key, h => h.Value);
        }

        private string? SanitizeRequestBody(string? body, string? contentType)
        {
            if (string.IsNullOrEmpty(body)) return null;
            if (body.Length > 4096) return body[..4096] + "... (truncated)";

            // JSON ise hassas alanları maskele
            if (contentType?.Contains("application/json") == true)
            {
                return MaskSensitiveJsonFields(body);
            }

            return body;
        }

        private string? SanitizeResponseBody(string? body, string? contentType)
        {
            if (string.IsNullOrEmpty(body)) return null;
            if (body.Length > 1024) return body[..1024] + "... (truncated)";
            return body;
        }

        private string MaskSensitiveJsonFields(string json)
        {
            try
            {
                var sensitiveFields = new[] { "password", "token", "secret", "key", "cardNumber", "cvv" };
                var jsonDoc = JsonDocument.Parse(json);
                // Basit maskeleme - production'da daha sophisticated olabilir
                return json; // Şimdilik olduğu gibi dön, ihtiyaç halinde geliştirebiliriz
            }
            catch
            {
                return json;
            }
        }

        private string GetClientIP(HttpContext context)
        {
            var xForwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(xForwardedFor))
            {
                return xForwardedFor.Split(',')[0].Trim();
            }

            var xRealIp = context.Request.Headers["X-Real-IP"].FirstOrDefault();
            if (!string.IsNullOrEmpty(xRealIp))
            {
                return xRealIp;
            }

            return context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        }

        private string? GetMaskedApiKey(string? apiKey)
        {
            if (string.IsNullOrEmpty(apiKey)) return null;
            if (apiKey.Length <= 8) return "****";
            return apiKey[..4] + "***" + apiKey[^4..];
        }
    }

    // Yardımcı sınıflar
    public class RequestInfo
    {
        public string RequestId { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
        public string? Path { get; set; }
        public string? QueryString { get; set; }
        public Dictionary<string, IEnumerable<string>> Headers { get; set; } = new();
        public string? Body { get; set; }
        public string ClientIP { get; set; } = string.Empty;
        public string? UserAgent { get; set; }
        public DateTime Timestamp { get; set; }
        public string? UserId { get; set; }
        public string? ApiKey { get; set; }
    }

    public class ResponseInfo
    {
        public int StatusCode { get; set; }
        public Dictionary<string, IEnumerable<string>> Headers { get; set; } = new();
        public string? Body { get; set; }
        public long ContentLength { get; set; }
    }
}
