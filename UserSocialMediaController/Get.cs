using Core.Models.Read;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.UserSocialMediaController
{
    public partial class UserSocialMediaController
    {
        [HttpGet]
        public async Task<IActionResult> Get(long contentCreatorId, int? pageNumber = 1, int? pageSize = 10)
        {
            var result = await _service.Get(x => x.UserId == contentCreatorId && x.IsActive, null, pageNumber, pageSize);

            return Ok(new SuccessDataResult<List<UserSocialMediaRD>>(result.Data, result.Message, result.RecordTotals));
        }
    }
}
