using Core.Constants;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Claims;

namespace Api.Controllers.v1.UserSubscriptionPlanController
{
    public partial class UserSubscriptionPlanController
    {
        [Authorize(Roles = "")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
                return BadRequest(new ErrorDataResult<bool>(false, Messages.DeleteProcessFailed));

            var claims = _tokenHelper.GetClaimsFromToken(HttpContext);
            if (claims == null)
                return Unauthorized(Messages.UnAuthorize);

            var userId = claims.FindFirst(ClaimTypes.NameIdentifier).Value;

            bool result = await _service.Delete(id, long.Parse(userId));

            if (result)
                return Ok(new SuccessDataResult<bool>(true, Messages.DeleteProcessSuccessful));
            else
                return BadRequest(new ErrorDataResult<bool>(false, Messages.DeleteProcessFailed));
        }
    }
}
