using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.LanguageController;

public partial class LanguageController
{
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(LanguageWD model)
    {
        var resultModel = await _service.Create(model);

        if (resultModel.Success)
            return Ok(new SuccessDataResult<LanguageRD>(resultModel.Data, resultModel.Message));
        else
            return BadRequest(new ErrorDataResult<LanguageRD>(resultModel.Message));
    }
}
