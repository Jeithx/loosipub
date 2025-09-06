using Core.Constants;
using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Claims;

namespace Api.Controllers.v1.SubscriberController
{
    public partial class SubscriberController
    {
        [Authorize(Roles = "Fan, ContentCreator")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] SubscriberWD model)
        {
            var claims = _tokenHelper.GetClaimsFromToken(HttpContext);
            if (claims == null)
                return Unauthorized(Messages.UnAuthorize);

            var userId = claims.FindFirst(ClaimTypes.NameIdentifier).Value;

            model.UserId = long.Parse(userId);
            var result = await _service.Update(model);

            if (result.Success)
                return Ok(new SuccessDataResult<SubscriberRD>(result.Data, result.Message));
            else
                return BadRequest(new ErrorDataResult<SubscriberRD>(result.Message));
        }
    }
}
