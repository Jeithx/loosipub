using Core.Constants;
using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers.v1.WalletController
{
    public partial class WalletController
    {
        [Authorize(Roles = "Fan")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] WalletWD model)
        {
            var claims = _tokenHelper.GetClaimsFromToken(HttpContext);
            if (claims == null)
                return Unauthorized(Messages.UnAuthorize);

            var userId = claims.FindFirst(ClaimTypes.NameIdentifier).Value;

            model.UserId = long.Parse(userId);
            var result = await _service.Update(model);

            if (result.Success)
                return Ok(new SuccessDataResult<WalletRD>(result.Data, result.Message));
            else
                return BadRequest(new ErrorDataResult<WalletRD>(result.Message));
        }
    }
}