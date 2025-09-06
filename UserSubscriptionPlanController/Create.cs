using Core.Constants;
using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Claims;

namespace Api.Controllers.v1.UserSubscriptionPlanController;

public partial class UserSubscriptionPlanController
{
    [Authorize(Roles = "ContentCreator, Fan")]
    [HttpPost]
    public async Task<IActionResult> Create(UserSubscriptionPlanWD model)
    {
        var claims = _tokenHelper.GetClaimsFromToken(HttpContext);
        if (claims == null)
            return Unauthorized(Messages.UnAuthorize);

        var userId = claims.FindFirst(ClaimTypes.NameIdentifier).Value;

        model.ContentCreatorUserId = long.Parse(userId);

        var resultModel = await _service.Create(model);

        if (resultModel.Success)
            return Ok(new SuccessDataResult<UserSubscriptionPlanRD>(resultModel.Data, resultModel.Message));
        else
            return BadRequest(new ErrorDataResult<UserSubscriptionPlanRD>(resultModel.Message));
    }
}
