using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.FaqController;

public partial class FaqController
{
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(FaqWD model)
    {
        var resultModel = await _service.Create(model);

        if (resultModel.Success)
            return Ok(new SuccessDataResult<FaqRD>(resultModel.Data, resultModel.Message));
        else
            return BadRequest(new ErrorDataResult<FaqRD>(resultModel.Message));
    }
}
