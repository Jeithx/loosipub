using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.FaqController
{
    public partial class FaqController
    {
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] FaqWD model)
        {
            var result = await _service.Update(model);

            if (result.Success)
                return Ok(new SuccessDataResult<FaqRD>(result.Data, result.Message));
            else
                return BadRequest(new ErrorDataResult<FaqRD>(result.Message));
        }
    }
}
