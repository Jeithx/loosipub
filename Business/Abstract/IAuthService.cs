using Core.Entities.Concrete;
using Core.Entities.DTO;
using Core.Utilities.Results;
using Core.Utilities.Security.Jwt;
using Entities.Dtos;

namespace Business.Abstract
{
    public interface IAuthService
    {
        Task<IDataResult<ReturnAdminDTO>> AdminLogin(UserForLoginDTO model);
        Task<IDataResult<ReturnContentCreatorDTO>> ContentCreatorLogin(UserForLoginDTO model);
        Task<IDataResult<ReturnFanDTO>> FanLogin(UserForLoginDTO model);

        Task<IDataResult<ReturnAdminDTO>> AdminRegister(UserForRegisterDTO model);
        Task<IDataResult<ReturnContentCreatorDTO>> ContentCreatorRegister(UserForRegisterDTO model);
        Task<IDataResult<ReturnFanDTO>> FanRegister(UserForRegisterDTO model);

        Task<AccessToken> CreateAccessToken(User appUser, string userAgent, string ipAdress);
    }
}
