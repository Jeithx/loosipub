using Core.Models.Read;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.SubscriberController
{
    public partial class SubscriberController
    {
        [Authorize(Roles = "ContentCreator, Fan")]
        [HttpGet("GetByContentCreatorSubscriber")]
        public async Task<IActionResult> GetByContentCreatorSubscriber(long contentCreatorId, int? pageNumber = 1, int? pageSize = 10)
        {
            var result = await _service.Get(x => x.UserId == contentCreatorId && x.IsActive, new[] { "User" }, pageNumber, pageSize);
            return Ok(new SuccessDataResult<List<SubscriberRD>>(result.Data, result.Message, result.RecordTotals));
        }
    }
}
