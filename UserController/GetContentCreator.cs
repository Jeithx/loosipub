using Core.Constants;
using Core.Models.Read;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Claims;
namespace API.Controllers.v1.UserController
{
    public partial class UserController
    {
        [HttpGet("GetContentCreator")]
        public async Task<IActionResult> GetContentCreator(int? pageNumber = 1, int? pageSize = 10)
        {
            var claims = _tokenHelper.GetClaimsFromToken(HttpContext);
            if (claims == null)
                return Unauthorized(Messages.UnAuthorize);

            var userId = claims.FindFirst(ClaimTypes.NameIdentifier).Value;

            var result = await _service.Get(x => x.UserTypeId == 1 && x.IsActive, null, pageNumber, pageSize);

            return Ok(new SuccessDataResult<List<UserRD>>(result.Data, result.Message, result.RecordTotals));
        }
    }
}
