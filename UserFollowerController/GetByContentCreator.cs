using Core.Models.Read;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.UserFollowerController
{
    public partial class UserFollowerController
    {
        [HttpGet("GetByContentCreator")]
        public async Task<IActionResult> GetByContentCreator(long contentCreatorId, int? pageNumber = 1, int? pageSize = 10)
        {
            var result = await _service.Get(x => x.IsActive && x.FollowedId == contentCreatorId, new[] { "Followed" }, pageNumber, pageSize);

            return Ok(new SuccessDataResult<List<UserFollowerRD>>(result.Data, result.Message, result.RecordTotals));
        }
    }
}
