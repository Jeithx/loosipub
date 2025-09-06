using Core.Constants;
using Core.Models.Read;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.LanguageController
{
    public partial class LanguageController
    {
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(long id)
        {
            if (id == 0)
                return BadRequest(new ErrorDataResult<LanguageRD>(Messages.NotFound));

            var result = await _service.GetByExpression(x => x.Id == id);

            if (result == null)
                return BadRequest(new ErrorDataResult<LanguageRD>(Messages.NotFound));

            return Ok(new SuccessDataResult<LanguageRD>(result, Messages.GetProcessSuccessful));
        }
    }
}