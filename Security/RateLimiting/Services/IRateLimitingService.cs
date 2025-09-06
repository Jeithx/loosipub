using Core.Utilities.Security.RateLimiting.Models;

namespace Core.Utilities.Security.RateLimiting.Services;

public interface IRateLimitingService
{
    Task<RateLimitResult> CheckRateLimitAsync(string clientId, string endpoint);
    Task ResetRateLimitAsync(string clientId);
    Task<Dictionary<string, RateLimitInfo>> GetRateLimitStatsAsync();
}