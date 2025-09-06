using Core.Constants;
using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Claims;

namespace API.Controllers.v1.UserController
{
    public partial class UserController
    {
        [HttpPut("ProfilePhotoUpdate")]
        public async Task<IActionResult> ProfilePhotoUpdate([FromForm] FileWD model)
        {
            var claims = _tokenHelper.GetClaimsFromToken(HttpContext);
            if (claims == null)
                return Unauthorized(Messages.UnAuthorize);

            var userId = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
            var image = await _fileManagerHelper.UploadIFormFileAsync(model.File, $"Loosip/ProfilePhoto/{userId.ToString()}");
            var id = long.Parse(userId);
            var result = await _service.UpdateProfilePhoto(id, image);

            if (result.Success)
                return Ok(new SuccessDataResult<UserRD>(result.Data, result.Message));
            else
                return BadRequest(new ErrorDataResult<UserRD>(result.Message));
        }
    }
}
