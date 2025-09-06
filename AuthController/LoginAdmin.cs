using API.Helpers;
using Core.Constants;
using Core.Entities.DTO;
using Core.Enums;
using Core.Utilities.Results;
using Core.Utilities.Security.Jwt;
using Entities.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.AuthController;

public partial class AuthController
{
    [HttpPost("LoginAdmin")]
    public async Task<IActionResult> LoginAdmin([FromBody] UserForLoginDTO dto)
    {
        try
        {
            var result = await _service.AdminLogin(dto);
            if (result.Success)
            {
                var ipAdress = GetRequestDetailHelper.GetClientIpAddress(Request);
                var userAgent = GetRequestDetailHelper.GetUserAgent(Request);

                var accessToken = _tokenHelper.CreateToken(new Core.Entities.Concrete.User
                {
                    Email = result.Data.Email,
                    Id = result.Data.Id,
                    UserAgent = userAgent,
                    DisplayName = result.Data.DisplayName,
                    Username = result.Data.UserName,
                    UserTypeId = (int)EUserType.Admin
                }, null);

                return Ok(new SuccessDataResult<AccessToken>(accessToken, result.Message));
            }

            return BadRequest(new ErrorDataResult<ReturnAdminDTO>(result.Message));
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorDataResult<ReturnAdminDTO>(Messages.ProcessFailed));
        }
    }
}