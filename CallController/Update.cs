using Core.Constants;
using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Claims;

namespace Api.Controllers.v1.CallController
{
    public partial class CallController
    {
        [Authorize(Roles = "ContentCreator, Fan")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CallWD model)
        {
            var claims = _tokenHelper.GetClaimsFromToken(HttpContext);
            if (claims == null)
                return Unauthorized(Messages.UnAuthorize);

            var userId = claims.FindFirst(ClaimTypes.NameIdentifier).Value;

            model.CallerUserId = long.Parse(userId);

            var result = await _service.Update(model);

            if (result.Success)
                return Ok(new SuccessDataResult<CallRD>(result.Data, result.Message));
            else
                return BadRequest(new ErrorDataResult<CallRD>(result.Message));
        }
    }
}
