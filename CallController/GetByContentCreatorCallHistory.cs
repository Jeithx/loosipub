using Core.Constants;
using Core.Models.Read;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Claims;

namespace Api.Controllers.v1.CallController
{
    public partial class CallController
    {
        [Authorize(Roles = "ContentCreator")]
        [HttpGet("GetByContentCreatorCallHistory")]
        public async Task<IActionResult> GetByContentCreatorCallHistory(int? pageNumber = 1, int? pageSize = 10)
        {
            var claims = _tokenHelper.GetClaimsFromToken(HttpContext);
            if (claims == null)
                return Unauthorized(Messages.UnAuthorize);

            var userId = claims.FindFirst(ClaimTypes.NameIdentifier).Value;

            var result = await _service.Get(x => x.IsActive && x.ReceiverUserId == long.Parse(userId), new[] { "CallerUser", "ReceiverUser" }, pageNumber, pageSize);

            return Ok(new SuccessDataResult<List<CallRD>>(result.Data, result.Message, result.RecordTotals));
        }
    }
}
