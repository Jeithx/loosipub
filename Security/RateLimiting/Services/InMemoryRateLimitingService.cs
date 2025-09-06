using Core.Utilities.Security.RateLimiting.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Core.Utilities.Security.RateLimiting.Services
{
    public class InMemoryRateLimitingService : IRateLimitingService
    {
        private readonly ConcurrentDictionary<string, RateLimitInfo> _rateLimits = new();
        private readonly ILogger<InMemoryRateLimitingService> _logger;
        private readonly RateLimitOptions _options;

        public InMemoryRateLimitingService(ILogger<InMemoryRateLimitingService> logger, RateLimitOptions options)
        {
            _logger = logger;
            _options = options;

            // Background temizlik task'i başlat
            _ = Task.Run(CleanupExpiredEntriesAsync);
        }

        public async Task<RateLimitResult> CheckRateLimitAsync(string clientId, string endpoint)
        {
            var key = $"{clientId}:{endpoint}";
            var now = DateTime.UtcNow;

            var rateLimitInfo = _rateLimits.AddOrUpdate(key,
                // Yeni entry oluştur
                new RateLimitInfo
                {
                    RequestCount = 1,
                    WindowStart = now,
                    LastRequest = now,
                    IsBlocked = false
                },
                // Mevcut entry'yi güncelle
                (k, existing) => UpdateRateLimitInfo(existing, now));

            // Rate limit kontrolü
            var isAllowed = await IsRequestAllowedAsync(rateLimitInfo, endpoint);

            if (!isAllowed)
            {
                rateLimitInfo.IsBlocked = true;
                rateLimitInfo.BlockedUntil = now.AddMinutes(_options.BlockDurationMinutes);

                _logger.LogWarning("Rate limit aşıldı. Client: {ClientId}, Endpoint: {Endpoint}, RequestCount: {RequestCount}",
                    clientId, endpoint, rateLimitInfo.RequestCount);
            }

            return new RateLimitResult
            {
                IsAllowed = isAllowed,
                RequestCount = rateLimitInfo.RequestCount,
                WindowStart = rateLimitInfo.WindowStart,
                NextResetTime = rateLimitInfo.WindowStart.AddMinutes(_options.WindowSizeMinutes),
                RetryAfterSeconds = isAllowed ? 0 : (int)(rateLimitInfo.BlockedUntil?.Subtract(now).TotalSeconds ?? 0)
            };
        }

        private RateLimitInfo UpdateRateLimitInfo(RateLimitInfo existing, DateTime now)
        {
            // Eğer bloklu ise ve süre dolmadıysa
            if (existing.IsBlocked && existing.BlockedUntil > now)
            {
                return existing;
            }

            // Window süresi dolmuş mu kontrol et
            if (now.Subtract(existing.WindowStart).TotalMinutes >= _options.WindowSizeMinutes)
            {
                // Yeni window başlat
                existing.RequestCount = 1;
                existing.WindowStart = now;
                existing.IsBlocked = false;
                existing.BlockedUntil = null;
            }
            else
            {
                // Mevcut window'da request sayısını artır
                existing.RequestCount++;
                existing.IsBlocked = false; // Block süresi dolmuşsa reset et
            }

            existing.LastRequest = now;
            return existing;
        }

        private async Task<bool> IsRequestAllowedAsync(RateLimitInfo rateLimitInfo, string endpoint)
        {
            // Endpoint bazlı limitleri kontrol et
            var endpointLimits = GetEndpointLimits(endpoint);

            // Eğer bloklu ise ve süre dolmadıysa
            if (rateLimitInfo.IsBlocked && rateLimitInfo.BlockedUntil > DateTime.UtcNow)
            {
                return false;
            }

            return rateLimitInfo.RequestCount <= endpointLimits.MaxRequests;
        }

        private EndpointLimits GetEndpointLimits(string endpoint)
        {
            // Farklı endpoint'ler için farklı limitler
            return endpoint.ToLowerInvariant() switch
            {
                var e when e.Contains("/auth/login") => new EndpointLimits { MaxRequests = _options.AuthEndpointLimit },
                var e when e.Contains("/auth/register") => new EndpointLimits { MaxRequests = _options.AuthEndpointLimit },
                var e when e.Contains("/api/products") && e.Contains("post") => new EndpointLimits { MaxRequests = _options.WriteEndpointLimit },
                var e when e.Contains("/api/orders") && e.Contains("post") => new EndpointLimits { MaxRequests = _options.WriteEndpointLimit },
                _ => new EndpointLimits { MaxRequests = _options.GeneralEndpointLimit }
            };
        }

        public async Task ResetRateLimitAsync(string clientId)
        {
            var keysToRemove = _rateLimits.Keys.Where(k => k.StartsWith($"{clientId}:")).ToList();

            foreach (var key in keysToRemove)
            {
                _rateLimits.TryRemove(key, out _);
            }

            _logger.LogInformation("Rate limit resetlendi. Client: {ClientId}", clientId);
            await Task.CompletedTask;
        }

        public async Task<Dictionary<string, RateLimitInfo>> GetRateLimitStatsAsync()
        {
            return await Task.FromResult(_rateLimits.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
        }

        private async Task CleanupExpiredEntriesAsync()
        {
            while (true)
            {
                try
                {
                    var now = DateTime.UtcNow;
                    var expiredKeys = _rateLimits
                        .Where(kvp => now.Subtract(kvp.Value.LastRequest).TotalMinutes > _options.WindowSizeMinutes * 2)
                        .Select(kvp => kvp.Key)
                        .ToList();

                    foreach (var key in expiredKeys)
                    {
                        _rateLimits.TryRemove(key, out _);
                    }

                    if (expiredKeys.Count > 0)
                    {
                        _logger.LogDebug("Rate limit cache temizlendi. Silinen entry sayısı: {Count}", expiredKeys.Count);
                    }

                    await Task.Delay(TimeSpan.FromMinutes(5)); // 5 dakikada bir temizle
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Rate limit cache temizliği sırasında hata oluştu");
                    await Task.Delay(TimeSpan.FromMinutes(1));
                }
            }
        }
    }

}
