using Core.Constants;
using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Claims;

namespace Api.Controllers.v1.ReportController;

public partial class ReportController
{
    [Authorize(Roles = "ContentCreator, Fan")]
    [HttpPost]
    public async Task<IActionResult> Create(ReportWD model)
    {
        var claims = _tokenHelper.GetClaimsFromToken(HttpContext);
        if (claims == null)
            return Unauthorized(Messages.UnAuthorize);

        var userId = claims.FindFirst(ClaimTypes.NameIdentifier).Value;

        model.ReporterId = long.Parse(userId);
        var resultModel = await _service.Create(model);

        if (resultModel.Success)
            return Ok(new SuccessDataResult<ReportRD>(resultModel.Data, resultModel.Message));
        else
            return BadRequest(new ErrorDataResult<ReportRD>(resultModel.Message));
    }
}
