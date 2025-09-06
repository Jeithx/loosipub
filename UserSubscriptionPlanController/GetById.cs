using Core.Constants;
using Core.Models.Read;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.UserSubscriptionPlanController
{
    public partial class UserSubscriptionPlanController
    {

        [Authorize(Roles = "ContentCreator, Fan")]
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(long id)
        {
            if (id == 0)
                return BadRequest(new ErrorDataResult<UserSubscriptionPlanRD>(Messages.NotFound));

            var result = await _service.GetByExpression(x => x.Id == id && x.IsActive);

            if (result == null)
                return BadRequest(new ErrorDataResult<UserSubscriptionPlanRD>(Messages.NotFound));

            return Ok(new SuccessDataResult<UserSubscriptionPlanRD>(result, Messages.GetProcessSuccessful));
        }
    }
}