using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.PostMediaController
{
    public partial class PostMediaController
    {
        [Authorize(Roles = "ContentCreator, Fan")]
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] PostMediaWD model)
        {
            if (model.File != null)
            {
                //TODO: dosyanýn upload edilmesi ve kaydedilmesi iþlemleri yapýlacak
            }

            var result = await _service.Update(model);

            if (result.Success)
                return Ok(new SuccessDataResult<PostMediaRD>(result.Data, result.Message));
            else
                return BadRequest(new ErrorDataResult<PostMediaRD>(result.Message));
        }
    }
}
