using Core.Constants;
using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Claims;

namespace Api.Controllers.v1.UserFollowerController;

public partial class UserFollowerController
{
    [Authorize(Roles = "ContentCreator, Fan")]
    [HttpPost("Follow")]
    public async Task<IActionResult> Follow(UserFollowerWD model)
    {
        var claims = _tokenHelper.GetClaimsFromToken(HttpContext);
        if (claims == null)
            return Unauthorized(Messages.UnAuthorize);

        var userId = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
        model.IsActive = true;
        model.FollowerId = long.Parse(userId);
        var resultModel = await _service.Create(model);

        if (resultModel.Success)
            return Ok(new SuccessDataResult<UserFollowerRD>(resultModel.Data, resultModel.Message));
        else
            return BadRequest(new ErrorDataResult<UserFollowerRD>(resultModel.Message));
    }
}
