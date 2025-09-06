using Core.Constants;
using Core.Models.Read;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.CountryController
{
    public partial class CountryController
    {
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(long id)
        {
            if (id == 0)
                return BadRequest(new ErrorDataResult<CountryRD>(Messages.NotFound));

            var result = await _service.GetByExpression(x => x.Id == id);

            if (result == null)
                return BadRequest(new ErrorDataResult<CountryRD>(Messages.NotFound));

            return Ok(new SuccessDataResult<CountryRD>(result, Messages.GetProcessSuccessful));
        }
    }
}