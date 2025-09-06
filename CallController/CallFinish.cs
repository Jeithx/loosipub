using Core.Constants;
using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Claims;

namespace Api.Controllers.v1.CallController;

public partial class CallController
{
    [Authorize(Roles = "ContentCreator, Fan")]
    [HttpPost("CallFinish")]
    public async Task<IActionResult> CallFinish(CallWD model)
    {
        var claims = _tokenHelper.GetClaimsFromToken(HttpContext);
        if (claims == null)
            return Unauthorized(Messages.UnAuthorize);

        var userId = claims.FindFirst(ClaimTypes.NameIdentifier).Value;

        model.CallerUserId = long.Parse(userId);
        //model.Status = (int)ECallStatus.Completed; //Dýþarýdan gelecek yanýta göre ayarlanacak
        var resultModel = await _service.Update(model);

        if (resultModel.Success)
            return Ok(new SuccessDataResult<CallRD>(resultModel.Data, resultModel.Message));
        else
            return BadRequest(new ErrorDataResult<CallRD>(resultModel.Message));
    }
}
