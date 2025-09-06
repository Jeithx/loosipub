using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.GenderController;

public partial class GenderController
{
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(GenderWD model)
    {
        var resultModel = await _service.Create(model);

        if (resultModel.Success)
            return Ok(new SuccessDataResult<GenderRD>(resultModel.Data, resultModel.Message));
        else
            return BadRequest(new ErrorDataResult<GenderRD>(resultModel.Message));
    }
}
