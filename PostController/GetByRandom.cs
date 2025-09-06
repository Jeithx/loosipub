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
        /// <summary>
        /// Postlarý rastgele getirir. Keþfet sekmesi için kullanýlýr.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [Authorize(Roles = "ContentCreator, Fan")]
        [HttpGet("GetByRandom")]
        public async Task<IActionResult> GetByRandom(int? pageNumber = 1, int? pageSize = 10)
        {
            var claims = _tokenHelper.GetClaimsFromToken(HttpContext);
            if (claims == null)
                return Unauthorized(Messages.UnAuthorize);

            var userId = claims.FindFirst(ClaimTypes.NameIdentifier).Value;

            var result = await _service.GetByRandom(pageNumber, pageSize, long.Parse(userId));

            return Ok(new SuccessDataResult<List<PostRD>>(result.Data, result.Message, result.RecordTotals));
        }
    }
}
