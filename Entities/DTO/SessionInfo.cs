using Core.Utilities.Security.Jwt;

namespace Core.Entities.DTO;

public class SessionInfo
{
    public long Id { get; set; }
    public string UserFullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public AccessToken AccessToken { get; set; } = null!;
}