using Core.Constants;
using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Claims;

namespace Api.Controllers.v1.ReportController
{
    public partial class ReportController
    {
        [Authorize(Roles = "ContentCreator, Fan, Admin")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ReportWD model)
        {
            var claims = _tokenHelper.GetClaimsFromToken(HttpContext);
            if (claims == null)
                return Unauthorized(Messages.UnAuthorize);

            var userId = claims.FindFirst(ClaimTypes.NameIdentifier).Value;

            model.ReporterId = long.Parse(userId);
            var result = await _service.Update(model);

            if (result.Success)
                return Ok(new SuccessDataResult<ReportRD>(result.Data, result.Message));
            else
                return BadRequest(new ErrorDataResult<ReportRD>(result.Message));
        }
    }
}
