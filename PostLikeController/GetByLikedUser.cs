using Core.Constants;
using Core.Models.Read;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Claims;

namespace Api.Controllers.v1.PostLikeController;

public partial class PostLikeController
{
    [Authorize(Roles = "ContentCreator, Fan")]
    [HttpGet("GetByLikedUser")]
    public async Task<IActionResult> GetByLikedUser()
    {
        var claims = _tokenHelper.GetClaimsFromToken(HttpContext);
        if (claims == null)
            return Unauthorized(Messages.UnAuthorize);

        var userId = claims.FindFirst(ClaimTypes.NameIdentifier).Value;

        var resultModel = await _service.Get(x => x.IsActive && x.UserId == long.Parse(userId), new[] { "Post", "Post.PostMedias", "Post.PostTags" });

        if (resultModel.Success)
            return Ok(new SuccessDataResult<List<PostLikeRD>>(resultModel.Data, resultModel.Message));
        else
            return BadRequest(new ErrorDataResult<List<PostLikeRD>>(resultModel.Message));
    }
}
