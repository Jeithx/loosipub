using Core.Constants;
using Core.Models.Read;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.PostController
{
    public partial class PostController
    {
        [Authorize(Roles = "ContentCreator, Fan")]
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(long id)
        {
            if (id == 0)
                return BadRequest(new ErrorDataResult<PostRD>(Messages.NotFound));

            var result = await _service.GetByExpression(x => x.Id == id, new[] { "PostMedia", "PostTags", "PostComments" });

            if (result == null)
                return BadRequest(new ErrorDataResult<PostRD>(Messages.NotFound));

            return Ok(new SuccessDataResult<PostRD>(result, Messages.GetProcessSuccessful));
        }
    }
}