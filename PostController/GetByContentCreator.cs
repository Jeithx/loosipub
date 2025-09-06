using Core.Models.Read;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.PostController
{
    public partial class PostController
    {
        [Authorize(Roles = "ContentCreator, Fan")]
        [HttpGet("GetByContentCreator")]
        public async Task<IActionResult> GetByContentCreator(long contentCreatorId, int? pageNumber = 1, int? pageSize = 10)
        {
            var result = await _service.Get(x => x.IsActive && x.UserId == contentCreatorId, new[] { "PostMedia", "PostTags" }, pageNumber, pageSize);

            return Ok(new SuccessDataResult<List<PostRD>>(result.Data, result.Message, result.RecordTotals));
        }
    }
}
