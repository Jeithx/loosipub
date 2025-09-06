namespace Core.Utilities.Security.RateLimiting.Models;

public class RateLimitResult
{
    public bool IsAllowed { get; set; }
    public int RequestCount { get; set; }
    public DateTime WindowStart { get; set; }
    public DateTime NextResetTime { get; set; }
    public int RetryAfterSeconds { get; set; }
}