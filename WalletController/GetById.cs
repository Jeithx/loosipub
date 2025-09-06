using Core.Constants;
using Core.Models.Read;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers.v1.WalletController
{
    public partial class WalletController
    {
        [Authorize(Roles = "Fan")]
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById()
        {
            var claims = _tokenHelper.GetClaimsFromToken(HttpContext);
            if (claims == null)
                return Unauthorized(Messages.UnAuthorize);

            var userId = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _service.GetByExpression(x => x.IsActive && x.UserId == long.Parse(userId), null);

            return Ok(new SuccessDataResult<WalletRD>(result, Messages.GetProcessSuccessful));
        }
    }
}