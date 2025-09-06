using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Core.Constants;
using Core.Models;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Core.Models.Read;

namespace Api.Controllers.v1.WalletLogController
{
    public partial class WalletLogController
    {
        [Authorize(Roles = "Fan")]
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int? pageNumber = 1, int? pageSize = 10)
        {
            var claims = _tokenHelper.GetClaimsFromToken(HttpContext);
            if (claims == null)
                return Unauthorized(Messages.UnAuthorize);

            var userId = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _service.Get(x => x.Wallet.UserId == long.Parse(userId) && x.IsActive,
                new[] { "Wallet" }, pageNumber, pageSize);

            if (result.Data?.Count > 0)
                return BadRequest(new ErrorDataResult<List<WalletLogRD>>(Messages.NotFound));

            return Ok(new SuccessDataResult<List<WalletLogRD>>(result.Data, result.Message));
        }
    }
}