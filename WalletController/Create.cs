using System.Security.Claims;
using Core.Constants;
using Microsoft.AspNetCore.Mvc;
using Core.Models.Write;
using Core.Models;
using Core.Utilities.Results;
using Core.Models.Read;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers.v1.WalletController;

public partial class WalletController
{
    [Authorize(Roles = "Fan")]
    [HttpPost]
    public async Task<IActionResult> Create(WalletWD model)
    {
        var claims = _tokenHelper.GetClaimsFromToken(HttpContext);
        if (claims == null)
            return Unauthorized(Messages.UnAuthorize);

        var userId = claims.FindFirst(ClaimTypes.NameIdentifier).Value;

        model.UserId = long.Parse(userId);
        var resultModel = await _service.Create(model);

        if (resultModel.Success)
            return Ok(new SuccessDataResult<WalletRD>(resultModel.Data, resultModel.Message));
        else
            return BadRequest(new ErrorDataResult<WalletRD>(resultModel.Message));
    }
}