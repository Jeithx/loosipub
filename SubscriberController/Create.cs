using Core.Constants;
using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Claims;

namespace Api.Controllers.v1.SubscriberController;

public partial class SubscriberController
{
    [Authorize(Roles = "ContentCreator, Fan")]
    [HttpPost]
    public async Task<IActionResult> Create(SubscriberWD model)
    {
        var claims = _tokenHelper.GetClaimsFromToken(HttpContext);
        if (claims == null)
            return Unauthorized(Messages.UnAuthorize);

        var userId = claims.FindFirst(ClaimTypes.NameIdentifier).Value;

        model.UserId = long.Parse(userId);

        var resultModel = await _service.Create(model);

        if (resultModel.Success)
            return Ok(new SuccessDataResult<SubscriberRD>(resultModel.Data, resultModel.Message));
        else
            return BadRequest(new ErrorDataResult<SubscriberRD>(resultModel.Message));
    }
}
