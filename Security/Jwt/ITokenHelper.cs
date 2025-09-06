using Core.Entities.Concrete;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Core.Utilities.Security.Jwt
{
    public interface ITokenHelper
    {
        AccessToken CreateToken(User user, string? refreshToken = null);
        ClaimsPrincipal? GetClaimsFromToken(HttpContext httpContext);

        string DecodeToken(string token);

    }
}
