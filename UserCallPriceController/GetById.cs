using Core.Constants;
using Core.Models.Read;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Claims;

namespace Api.Controllers.v1.UserCallPriceController
{
    public partial class UserCallPriceController
    {
        [Authorize(Roles = "ContentCreator")]
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(long id)
        {
            if (id == 0)
                return BadRequest(new ErrorDataResult<UserCallPriceRD>(Messages.NotFound));

            var claims = _tokenHelper.GetClaimsFromToken(HttpContext);
            if (claims == null)
                return Unauthorized(Messages.UnAuthorize);

            var userId = claims.FindFirst(ClaimTypes.NameIdentifier).Value;

            var result = await _service.GetByExpression(x => x.Id == id && x.UserId == long.Parse(userId));

            if (result == null)
                return BadRequest(new ErrorDataResult<UserCallPriceRD>(Messages.NotFound));

            return Ok(new SuccessDataResult<UserCallPriceRD>(result, Messages.GetProcessSuccessful));
        }
    }
}