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
    [HttpPost("RegisterFan")]
    public async Task<IActionResult> RegisterFan([FromBody] UserForRegisterDTO dto)
    {
        try
        {
            var result = await _service.FanRegister(dto);
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
                    UserTypeId = (int)EUserType.Fan
                }, null);

                return Ok(new SuccessDataResult<AccessToken>(accessToken, result.Message));
            }

            return BadRequest(new ErrorDataResult<ReturnFanDTO>(result.Message));
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorDataResult<ReturnFanDTO>(Messages.ProcessFailed));
        }
    }
}