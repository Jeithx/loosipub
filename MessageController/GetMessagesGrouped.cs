using Core.Constants;
using Core.Utilities.Results;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Claims;

namespace Api.Controllers.v1.MessageController
{
    public partial class MessageController
    {
        [Authorize(Roles = "ContentCreator, Fan")]
        [HttpGet("GetMessagesGrouped")]
        public async Task<IActionResult> GetMessagesGrouped(int? pageNumber = 1, int? pageSize = 10)
        {
            var claims = _tokenHelper.GetClaimsFromToken(HttpContext);
            if (claims == null)
                return Unauthorized(Messages.UnAuthorize);

            var currentUserId = claims.FindFirst(ClaimTypes.NameIdentifier).Value;


            var result = await _service.GetGroupedMessages(long.Parse(currentUserId), pageNumber, pageSize);

            return Ok(new SuccessDataResult<List<DapperWithGetMessage>>(result.Data, result.Message, result.RecordTotals));
        }
    }
}
