namespace Core.Utilities.Security.RateLimiting.Models;

public class RateLimitOptions
{
    public int GeneralEndpointLimit { get; set; } = 100; // Genel endpoint'ler için dakikada 100 request
    public int AuthEndpointLimit { get; set; } = 5;      // Auth endpoint'leri için dakikada 5 request
    public int WriteEndpointLimit { get; set; } = 20;    // Write işlemleri için dakikada 20 request
    public int WindowSizeMinutes { get; set; } = 1;      // Pencere boyutu (dakika)
    public int BlockDurationMinutes { get; set; } = 5;   // Blok süresi (dakika)
    public List<string> WhitelistedIPs { get; set; } = new(); // Beyaz liste IP'ler
    public List<string> BlacklistedIPs { get; set; } = new(); // Kara liste IP'ler
}