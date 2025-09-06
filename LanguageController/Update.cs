using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.LanguageController
{
    public partial class LanguageController
    {
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] LanguageWD model)
        {
            var result = await _service.Update(model);

            if (result.Success)
                return Ok(new SuccessDataResult<LanguageRD>(result.Data, result.Message));
            else
                return BadRequest(new ErrorDataResult<LanguageRD>(result.Message));
        }
    }
}
