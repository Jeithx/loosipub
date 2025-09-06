using Core.Constants;
using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Claims;

namespace Api.Controllers.v1.PostLikeController;

public partial class PostLikeController
{
    [Authorize(Roles = "ContentCreator, Fan")]
    [HttpPost("Like")]
    public async Task<IActionResult> Like(PostLikeWD model)
    {
        var claims = _tokenHelper.GetClaimsFromToken(HttpContext);
        if (claims == null)
            return Unauthorized(Messages.UnAuthorize);

        var userId = claims.FindFirst(ClaimTypes.NameIdentifier).Value;

        model.UserId = long.Parse(userId);
        model.IsActive = true;
        model.CreationDate = DateTime.Now;
        var resultModel = await _service.Create(model);

        if (resultModel.Success)
            return Ok(new SuccessDataResult<PostLikeRD>(resultModel.Data, resultModel.Message));
        else
            return BadRequest(new ErrorDataResult<PostLikeRD>(resultModel.Message));
    }

}
