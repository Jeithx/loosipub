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
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UserWD model)
        {
            var claims = _tokenHelper.GetClaimsFromToken(HttpContext);
            if (claims == null)
                return Unauthorized(Messages.UnAuthorize);

            var userId = claims.FindFirst(ClaimTypes.NameIdentifier).Value;

            model.Id = long.Parse(userId);
            var result = await _service.Update(model);

            if (result.Success)
                return Ok(new SuccessDataResult<UserRD>(result.Data, result.Message));
            else
                return BadRequest(new ErrorDataResult<UserRD>(result.Message));
        }
    }
}
