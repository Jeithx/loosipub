using Core.Constants;
using Core.Models.Read;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Claims;

namespace Api.Controllers.v1.PostLikeController
{
    public partial class PostLikeController
    {
        /// <summary>
        /// Postu kimlerin beðendiðini gösterir.
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [Authorize(Roles = "ContentCreator, Fan")]
        [HttpGet("GetLikeUserList")]
        public async Task<IActionResult> GetLikeUserList(int postId, int pageNumber = 1, int pageSize = 10)
        {
            var claims = _tokenHelper.GetClaimsFromToken(HttpContext);
            if (claims == null)
                return Unauthorized(Messages.UnAuthorize);

            var userId = claims.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (postId == 0)
                return BadRequest(new ErrorDataResult<bool>(false, Messages.DeleteProcessFailed));

            var result = await _service.Get(x => x.PostId == postId && x.IsActive, new[] { "User" }, pageNumber, pageSize, true, long.Parse(userId));

            return Ok(new SuccessDataResult<List<PostLikeRD>>(result.Data, result.Message, result.RecordTotals));
        }
    }
}
