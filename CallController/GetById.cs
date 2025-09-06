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
        [Authorize(Roles = "ContentCreator, Fan")]
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(long id)
        {
            if (id == 0)
                return BadRequest(new ErrorDataResult<CallRD>(Messages.NotFound));

            var claims = _tokenHelper.GetClaimsFromToken(HttpContext);
            if (claims == null)
                return Unauthorized(Messages.UnAuthorize);

            var userId = claims.FindFirst(ClaimTypes.NameIdentifier).Value;

            var result = await _service.GetByExpression(x => x.Id == id && x.CallerUserId == long.Parse(userId), new[] { "CallerUser", "ReceiverUser" });

            if (result == null)
                return BadRequest(new ErrorDataResult<CallRD>(Messages.NotFound));

            return Ok(new SuccessDataResult<CallRD>(result, Messages.GetProcessSuccessful));
        }
    }
}