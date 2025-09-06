using Core.Constants;
using Core.Models.Read;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Claims;

namespace API.Controllers.v1.UserController
{
    public partial class UserController
    {
        [Authorize(Roles = "ContentCreator, Fan")]
        [HttpGet("GetByName")]
        public async Task<IActionResult> GetByName(string name)
        {
            var claims = _tokenHelper.GetClaimsFromToken(HttpContext);
            if (claims == null)
                return Unauthorized(Messages.UnAuthorize);

            var userId = claims.FindFirst(ClaimTypes.NameIdentifier).Value;

            var result = await _service.GetByExpression(x => x.UserName == name && x.IsActive, new[] { "Country" });
            if (result != null)
            {
                var userFollowed = await _userFollowerService.GetByExpression(x => x.FollowerId == long.Parse(userId) && x.FollowedId == result.Id && x.IsActive);

                if (userFollowed != null)
                    result.IsFollowed = true;
            }

            return Ok(new SuccessDataResult<UserRD>(result));
        }
    }
}