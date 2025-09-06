using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.SocialMediaController
{
    public partial class SocialMediaController
    {
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] SocialMediaWD model)
        {
            var result = await _service.Update(model);

            if (result.Success)
                return Ok(new SuccessDataResult<SocialMediaRD>(result.Data, result.Message));
            else
                return BadRequest(new ErrorDataResult<SocialMediaRD>(result.Message));
        }
    }
}
