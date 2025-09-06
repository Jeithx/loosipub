using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.CountryController;

public partial class CountryController
{
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(CountryWD model)
    {
        var resultModel = await _service.Create(model);

        if (resultModel.Success)
            return Ok(new SuccessDataResult<CountryRD>(resultModel.Data, resultModel.Message));
        else
            return BadRequest(new ErrorDataResult<CountryRD>(resultModel.Message));
    }
}
