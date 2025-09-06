using Core.Constants;
using Core.Models.Read;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.FaqController
{
    public partial class FaqController
    {

        [Authorize(Roles = "Admin")]
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(long id)
        {
            if (id == 0)
                return BadRequest(new ErrorDataResult<FaqRD>(Messages.NotFound));

            var result = await _service.GetByExpression(x => x.Id == id);

            if (result == null)
                return BadRequest(new ErrorDataResult<FaqRD>(Messages.NotFound));

            return Ok(new SuccessDataResult<FaqRD>(result, Messages.GetProcessSuccessful));
        }
    }
}