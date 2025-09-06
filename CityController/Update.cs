using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.CityController
{
    public partial class CityController
    {
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CityWD model)
        {
            var result = await _service.Update(model);

            if (result.Success)
                return Ok(new SuccessDataResult<CityRD>(result.Data, result.Message));
            else
                return BadRequest(new ErrorDataResult<CityRD>(result.Message));
        }
    }
}
