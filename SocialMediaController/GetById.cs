using Core.Constants;
using Core.Models.Read;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.SocialMediaController
{
    public partial class SocialMediaController
    {
        [Authorize(Roles = "Admin")]
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(long id)
        {
            if (id == 0)
                return BadRequest(new ErrorDataResult<SocialMediaRD>(Messages.NotFound));

            var result = await _service.GetByExpression(x => x.Id == id);

            if (result == null)
                return BadRequest(new ErrorDataResult<SocialMediaRD>(Messages.NotFound));

            return Ok(new SuccessDataResult<SocialMediaRD>(result, Messages.GetProcessSuccessful));
        }
    }
}