using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.SocialMediaController;

public partial class SocialMediaController
{
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(SocialMediaWD model)
    {
        var resultModel = await _service.Create(model);

        if (resultModel.Success)
            return Ok(new SuccessDataResult<SocialMediaRD>(resultModel.Data, resultModel.Message));
        else
            return BadRequest(new ErrorDataResult<SocialMediaRD>(resultModel.Message));
    }
}
