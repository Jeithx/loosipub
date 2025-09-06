using Core.Constants;
using Core.Models.Read;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers.v1.UserController
{
    public partial class UserController
    {
        [Authorize(Roles = "ContentCreator, Fan")]
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(long Id, int? pageNumber = 1, int? pageSize = 10)
        {
            var claims = _tokenHelper.GetClaimsFromToken(HttpContext);
            if (claims == null)
                return Unauthorized(Messages.UnAuthorize);

            var result = await _service.Get(x => x.Id == Id && x.IsActive, new[] { "Country", "City" }, pageNumber, pageSize);

            return Ok(new SuccessDataResult<List<UserRD>>(result.Data, result.Message, result.RecordTotals));
        }
    }
}
