using Core.Constants;
using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Claims;

namespace Api.Controllers.v1.UserSubscriptionPlanController
{
    public partial class UserSubscriptionPlanController
    {
        [Authorize(Roles = "ContentCreator")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UserSubscriptionPlanWD model)
        {
            var claims = _tokenHelper.GetClaimsFromToken(HttpContext);
            if (claims == null)
                return Unauthorized(Messages.UnAuthorize);

            var userId = claims.FindFirst(ClaimTypes.NameIdentifier).Value;

            model.ContentCreatorUserId = long.Parse(userId);
            var result = await _service.Update(model);

            if (result.Success)
                return Ok(new SuccessDataResult<UserSubscriptionPlanRD>(result.Data, result.Message));
            else
                return BadRequest(new ErrorDataResult<UserSubscriptionPlanRD>(result.Message));
        }
    }
}
