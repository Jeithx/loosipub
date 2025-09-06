using Core.Constants;
using Core.Models.Read;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Claims;

namespace Api.Controllers.v1.PostController
{
    public partial class PostController
    {
        [Authorize(Roles = "ContentCreator, Fan")]
        [HttpGet("GetByUserId")]
        public async Task<IActionResult> GetByUserId(long userId, int? pageNumber = 1, int? pageSize = 10)
        {
            if (userId == 0)
                return BadRequest(new ErrorDataResult<PostRD>(Messages.NotFound));

            var claims = _tokenHelper.GetClaimsFromToken(HttpContext);
            if (claims == null)
                return Unauthorized(Messages.UnAuthorize);

            var currentUserId = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _service.Get(x => x.UserId == userId && x.IsActive, new[] { "PostMedia", "PostTags", "User" }, pageNumber, pageSize, long.Parse(currentUserId));

            if (result == null)
                return BadRequest(new ErrorDataResult<PostRD>(Messages.NotFound));

            return Ok(new SuccessDataResult<List<PostRD>>(result.Data, result.Message, result.RecordTotals));
        }
    }
}