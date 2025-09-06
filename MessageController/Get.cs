using Core.Constants;
using Core.Models.Read;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Claims;

namespace Api.Controllers.v1.MessageController
{
    public partial class MessageController
    {
        [Authorize(Roles = "ContentCreator, Fan")]
        [HttpGet]
        public async Task<IActionResult> Get(long userId, int? pageNumber = 1, int? pageSize = 10)
        {
            var claims = _tokenHelper.GetClaimsFromToken(HttpContext);
            if (claims == null)
                return Unauthorized(Messages.UnAuthorize);

            var currentUserId = claims.FindFirst(ClaimTypes.NameIdentifier).Value;


            var result = await _service.Get(x => (x.SenderId == long.Parse(currentUserId) && x.ReceiverId == userId) ||(x.SenderId == userId && x.ReceiverId == long.Parse(currentUserId)), new[] { "Receiver", "Sender" }, pageNumber, pageSize);

            return Ok(new SuccessDataResult<List<MessageRD>>(result.Data, result.Message, result.RecordTotals));
        }
    }
}
