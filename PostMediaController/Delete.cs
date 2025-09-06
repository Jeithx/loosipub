using Core.Constants;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.PostMediaController
{
    public partial class PostMediaController
    {
        [Authorize(Roles = "ContentCreator, Fan")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
                return BadRequest(new ErrorDataResult<bool>(false, Messages.DeleteProcessFailed));

            bool result = await _service.Delete(id);

            if (result)
                return Ok(new SuccessDataResult<bool>(true, Messages.DeleteProcessSuccessful));
            else
                return BadRequest(new ErrorDataResult<bool>(false, Messages.DeleteProcessFailed));
        }
    }
}
