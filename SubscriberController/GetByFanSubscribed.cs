using Core.Constants;
using Core.Models.Read;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Claims;

namespace Api.Controllers.v1.SubscriberController
{
    public partial class SubscriberController
    {
        [Authorize(Roles = "ContentCreator, Fan")]
        [HttpGet("GetByFanSubscribed")]
        public async Task<IActionResult> GetByFanSubscribed(int? pageNumber = 1, int? pageSize = 10)
        {
            var claims = _tokenHelper.GetClaimsFromToken(HttpContext);
            if (claims == null)
                return Unauthorized(Messages.UnAuthorize);

            var userId = claims.FindFirst(ClaimTypes.NameIdentifier).Value;

            var result = await _service.Get(x => x.UserId == long.Parse(userId) && x.IsActive, new[] { "SubscriberNavigation" }, pageNumber, pageSize);

            return Ok(new SuccessDataResult<List<SubscriberRD>>(result.Data, result.Message, result.RecordTotals));
        }
    }
}
