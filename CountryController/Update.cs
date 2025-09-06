using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.CountryController
{
    public partial class CountryController
    {
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CountryWD model)
        {
            var result = await _service.Update(model);

            if (result.Success)
                return Ok(new SuccessDataResult<CountryRD>(result.Data, result.Message));
            else
                return BadRequest(new ErrorDataResult<CountryRD>(result.Message));
        }
    }
}
