using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.CityController;

public partial class CityController
{
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(CityWD model)
    {
        var resultModel = await _service.Create(model);

        if (resultModel.Success)
            return Ok(new SuccessDataResult<CityRD>(resultModel.Data, resultModel.Message));
        else
            return BadRequest(new ErrorDataResult<CityRD>(resultModel.Message));
    }
}
