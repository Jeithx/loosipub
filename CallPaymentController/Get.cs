using Core.Constants;
using Core.Enums;
using Core.Models.Read;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Claims;

namespace Api.Controllers.v1.CallPaymentController
{
    public partial class CallPaymentController
    {
        [Authorize(Roles = "ContentCreator, Fan")]
        [HttpGet]
        public async Task<IActionResult> Get(int? pageNumber = 1, int? pageSize = 10)
        {
            var claims = _tokenHelper.GetClaimsFromToken(HttpContext);
            if (claims == null)
                return Unauthorized(Messages.UnAuthorize);

            var userId = claims.FindFirst(ClaimTypes.NameIdentifier).Value;

            var result = await _service.Get(x => x.PaymentStatus == (int)EPaymentStatus.Completed && x.PayerId == long.Parse(userId), new[] { "Call", "Call.ReceiverUser" }, pageNumber, pageSize);

            return Ok(new SuccessDataResult<List<CallPaymentRD>>(result.Data, result.Message, result.RecordTotals));
        }
    }
}
