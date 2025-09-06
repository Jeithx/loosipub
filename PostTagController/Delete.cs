using Core.Constants;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.PostTagController
{
    public partial class PostTagController
    {
        [Authorize(Roles = "ContentCreator, Fan")]
        [HttpDelete]
        public async Task<IActionResult> Delete(long postId, long tagId)
        {
            if (postId == 0 || tagId == 0)
                return BadRequest(new ErrorDataResult<bool>(false, Messages.DeleteProcessFailed));

            bool result = await _service.Delete(postId, tagId);

            if (result)
                return Ok(new SuccessDataResult<bool>(true, Messages.DeleteProcessSuccessful));
            else
                return BadRequest(new ErrorDataResult<bool>(false, Messages.DeleteProcessFailed));
        }
    }
}
