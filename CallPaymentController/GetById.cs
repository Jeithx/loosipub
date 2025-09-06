using Core.Constants;
using Core.Models.Read;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.CallPaymentController
{
    public partial class CallPaymentController
    {
        [Authorize(Roles = "ContentCreator, Fan")]
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(long id)
        {
            if (id == 0)
                return BadRequest(new ErrorDataResult<CallPaymentRD>(Messages.NotFound));

            var result = await _service.GetByExpression(x => x.Id == id, new[] { "Call", "Payer" });

            if (result == null)
                return BadRequest(new ErrorDataResult<CallPaymentRD>(Messages.NotFound));

            return Ok(new SuccessDataResult<CallPaymentRD>(result, Messages.GetProcessSuccessful));
        }
    }
}