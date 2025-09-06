using Business.Abstract;
using Core.Constants;
using Core.Entities.DTO;
using Core.Enums;
using Core.Helpers;
using Core.Models.Write;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.Jwt;
using DataAccess.Abstract;
using Entities.Dtos;

namespace Business.Concrete;

public class AuthService : IAuthService
{
    private readonly ILogger _logger;
    private readonly ITokenHelper _tokenHelper;
    private readonly IUserDAL _userDal;
    readonly IUserLoginAttemptService _userLoginAttemptService;

    public AuthService(IUserDAL userDal, ITokenHelper tokenHelper, ILogger logger, IUserLoginAttemptService userLoginAttemptService)
    {
        _userDal = userDal;
        _tokenHelper = tokenHelper;
        _logger = logger;
        _userLoginAttemptService = userLoginAttemptService;
    }

    public async Task<AccessToken> CreateAccessToken(Core.Entities.Concrete.User appUser, string userAgent, string ipAdress)
    {
        var accessToken = _tokenHelper.CreateToken(new Core.Entities.Concrete.User
        {
            Email = appUser.Email,
            GenderId = appUser.GenderId,
            Id = appUser.Id,
            PhoneNumber = appUser.PhoneNumber,
            Username = appUser.Username,
            UserAgent = userAgent,
            DisplayName = appUser.DisplayName,
        });

        return accessToken;
    }

    public async Task<IDataResult<ReturnAdminDTO>> AdminLogin(UserForLoginDTO model)
    {
        try
        {
            var user = await _userDal.GetAsync(x =>
                x.IsActive == true
                && x.Email == model.Email && x.UserTypeId == (int)EUserType.Admin);
            if (user == null)
            {
                _logger.LogException(new Exception("User Not Found"), nameof(AdminLogin), new { data = model });
                return new ErrorDataResult<ReturnAdminDTO>(Messages.UserNotFound);
            }

            if (!HashingHelper.VerifyPasswordHash(model.Password, user.PasswordHash, user.PasswordSalt))
                return new ErrorDataResult<ReturnAdminDTO>(Messages.PasswordWrong);

            var adminUserRegisterDto = new ReturnAdminDTO
            {
                Email = user.Email,
                Id = user.Id,
                DisplayName = user.DisplayName,
                UserName = user.UserName,
            };

            return new SuccessDataResult<ReturnAdminDTO>(adminUserRegisterDto,
                Messages.LoginSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(AdminLogin), new { data = model });
            return new ErrorDataResult<ReturnAdminDTO>(Messages.LoginError);
        }
    }

    public async Task<IDataResult<ReturnContentCreatorDTO>> ContentCreatorLogin(UserForLoginDTO model)
    {
        try
        {
            var user = await _userDal.GetAsync(x =>
                x.IsActive == true
                && x.Email == model.Email && x.UserTypeId == (int)EUserType.ContentCreator);
            if (user == null)
            {
                _logger.LogException(new Exception("User Not Found"), nameof(AdminLogin), new { data = model });
                return new ErrorDataResult<ReturnContentCreatorDTO>(Messages.UserNotFound);
            }

            if (user.IsLocked == true)
                return new ErrorDataResult<ReturnContentCreatorDTO>(Messages.UserIsLocked);

            if (!HashingHelper.VerifyPasswordHash(model.Password, user.PasswordHash, user.PasswordSalt))
            {
                var dataCheck = await _userLoginAttemptService.Get(x => x.UserId == user.Id && x.Step == (int)ELoginStep.Login && x.Status == (int)ELoginStatus.WrongPassword && (x.AttemptDate.Day == DateTime.Now.Day && x.AttemptDate.Month == DateTime.Now.Month && x.AttemptDate.Year == DateTime.Now.Year));

                if (dataCheck.Success && dataCheck.Data != null)
                    if (dataCheck.Data.Count >= 3)
                    {
                        user.IsLocked = true;
                        await _userDal.UpdateAsync(user);

                        return new ErrorDataResult<ReturnContentCreatorDTO>(Messages.PasswordWrong);
                    }

                await _userLoginAttemptService.Create(new UserLoginAttemptWD
                {
                    UserId = user.Id,
                    AttemptDate = DateTime.Now,
                    Ipaddress = "",
                    Status = (int)ELoginStatus.WrongPassword,
                    Step = (int)ELoginStep.Login,
                    UserAgent = "",
                    AdditionalData = "Login attempt failed due to wrong password."
                });

                return new ErrorDataResult<ReturnContentCreatorDTO>(Messages.PasswordWrong);
            }

            var adminUserRegisterDto = new ReturnContentCreatorDTO
            {
                Email = user.Email,
                Id = user.Id,
                DisplayName = user.DisplayName,
                UserName = user.UserName,
            };

            return new SuccessDataResult<ReturnContentCreatorDTO>(adminUserRegisterDto,
                Messages.LoginSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(ContentCreatorLogin), new { data = model });
            return new ErrorDataResult<ReturnContentCreatorDTO>(Messages.LoginError);
        }
    }

    public async Task<IDataResult<ReturnFanDTO>> FanLogin(UserForLoginDTO model)
    {
        try
        {
            var user = await _userDal.GetAsync(x =>
                x.IsActive == true
                && x.Email == model.Email && x.UserTypeId == (int)EUserType.Fan);
            if (user == null)
            {
                _logger.LogException(new Exception("User Not Found"), nameof(AdminLogin), new { data = model });
                return new ErrorDataResult<ReturnFanDTO>(Messages.UserNotFound);
            }

            if (user.IsLocked == true)
                return new ErrorDataResult<ReturnFanDTO>(Messages.UserIsLocked);

            if (!HashingHelper.VerifyPasswordHash(model.Password, user.PasswordHash, user.PasswordSalt))
            {
                var dataCheck = await _userLoginAttemptService.Get(x => x.UserId == user.Id && x.Step == (int)ELoginStep.Login && x.Status == (int)ELoginStatus.WrongPassword && (x.AttemptDate.Day == DateTime.Now.Day && x.AttemptDate.Month == DateTime.Now.Month && x.AttemptDate.Year == DateTime.Now.Year));

                if (dataCheck.Success && dataCheck.Data != null)
                    if (dataCheck.Data.Count >= 3)
                    {
                        user.IsLocked = true;
                        await _userDal.UpdateAsync(user);

                        return new ErrorDataResult<ReturnFanDTO>(Messages.PasswordWrong);
                    }

                await _userLoginAttemptService.Create(new UserLoginAttemptWD
                {
                    UserId = user.Id,
                    AttemptDate = DateTime.Now,
                    Ipaddress = "",
                    Status = (int)ELoginStatus.WrongPassword,
                    Step = (int)ELoginStep.Login,
                    UserAgent = "",
                    AdditionalData = "Login attempt failed due to wrong password."
                });

                return new ErrorDataResult<ReturnFanDTO>(Messages.PasswordWrong);
            }

            var adminUserRegisterDto = new ReturnFanDTO
            {
                Email = user.Email,
                Id = user.Id,
                DisplayName = user.DisplayName,
                UserName = user.UserName,
            };

            return new SuccessDataResult<ReturnFanDTO>(adminUserRegisterDto,
                Messages.LoginSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(FanLogin), new { data = model });
            return new ErrorDataResult<ReturnFanDTO>(Messages.LoginError);
        }
    }

    public async Task<IDataResult<ReturnAdminDTO>> AdminRegister(UserForRegisterDTO model)
    {
        try
        {
            if (model.Password != model.ConfirmPassword)
                return new ErrorDataResult<ReturnAdminDTO>(Messages.PasswordWrong);

            var (ısValid, message) = PasswordValidator.IsStrongPassword(model.Password);
            if (!ısValid) return new ErrorDataResult<ReturnAdminDTO>(message);

            var userExists = await _userDal.GetAsync(x => x.Email == model.Email && x.IsActive && x.UserTypeId == (int)EUserType.Admin);

            if (userExists != null) return new ErrorDataResult<ReturnAdminDTO>(Messages.UserAlreadyExists);

            model.Email = model.Email.Trim().Replace(" ", "");

            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(model.Password, out passwordHash, out passwordSalt);

            var user = await _userDal.AddAsync(new Entities.Models.User
            {
                GenderId = 3,
                Email = model.Email,
                IsActive = true,
                BirthDate = model.BirthDate,
                CityId = model.CityId,
                CountryId = model.CountryId,
                CoverPictureUrl = null,
                CreationDate = DateTime.Now,
                DisplayName = model.DisplayName,
                IsVerified = false,
                LastLoginAt = DateTime.Now,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                PhoneNumber = model.Phone,
                PrefferedLanguageId = model.PrefferedLanguageId,
                Status = (int)EUserStatus.Approved,
                UserTypeId = (int)EUserType.Admin,
                UserName = model.UserName,
                ProfilePictureUrl = null,
                RefferalCode = null,
                UpdateDate = DateTime.Now,
                IsLocked = false,
            });

            var adminUserRegisterDto = new ReturnAdminDTO()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Id = user.Id,
                UserName = user.UserName
            };

            return new SuccessDataResult<ReturnAdminDTO>(adminUserRegisterDto,
                Messages.AddingProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(AdminRegister), new { data = model });
            return new ErrorDataResult<ReturnAdminDTO>(Messages.AddingProcessFailed);
        }
    }

    public async Task<IDataResult<ReturnContentCreatorDTO>> ContentCreatorRegister(UserForRegisterDTO model)
    {
        try
        {
            if (model.Password != model.ConfirmPassword)
                return new ErrorDataResult<ReturnContentCreatorDTO>(Messages.PasswordWrong);

            var (ısValid, message) = PasswordValidator.IsStrongPassword(model.Password);
            if (!ısValid) return new ErrorDataResult<ReturnContentCreatorDTO>(message);

            var userExists = await _userDal.GetAsync(x => x.Email == model.Email && x.IsActive && x.UserTypeId == (int)EUserType.Admin);

            if (userExists != null) return new ErrorDataResult<ReturnContentCreatorDTO>(Messages.UserAlreadyExists);

            model.Email = model.Email.Trim().Replace(" ", "");

            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(model.Password, out passwordHash, out passwordSalt);

            var user = await _userDal.AddAsync(new Entities.Models.User
            {
                GenderId = model.GenderId,
                Email = model.Email,
                IsActive = true,
                BirthDate = model.BirthDate,
                CityId = model.CityId,
                CountryId = model.CountryId,
                CoverPictureUrl = null,
                CreationDate = DateTime.Now,
                DisplayName = model.DisplayName,
                IsVerified = false,
                LastLoginAt = DateTime.Now,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                PhoneNumber = model.Phone,
                PrefferedLanguageId = model.PrefferedLanguageId,
                Status = (int)EUserStatus.AwaitingApproval,
                UserTypeId = (int)EUserType.ContentCreator,
                UserName = model.UserName,
                ProfilePictureUrl = null,
                RefferalCode = null,
                UpdateDate = DateTime.Now,
                IsLocked = false,
            });

            var userRegisterDTO = new ReturnContentCreatorDTO()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Id = user.Id,
                UserName = user.UserName,
            };

            return new SuccessDataResult<ReturnContentCreatorDTO>(userRegisterDTO,
                Messages.AddingProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(ContentCreatorRegister), new { data = model });
            return new ErrorDataResult<ReturnContentCreatorDTO>(Messages.AddingProcessFailed);
        }
    }

    public async Task<IDataResult<ReturnFanDTO>> FanRegister(UserForRegisterDTO model)
    {
        try
        {
            if (model.Password != model.ConfirmPassword)
                return new ErrorDataResult<ReturnFanDTO>(Messages.PasswordWrong);

            var (ısValid, message) = PasswordValidator.IsStrongPassword(model.Password);
            if (!ısValid) return new ErrorDataResult<ReturnFanDTO>(message);

            var userExists = await _userDal.GetAsync(x => x.Email == model.Email && x.IsActive && x.UserTypeId == (int)EUserType.Admin);

            if (userExists != null) return new ErrorDataResult<ReturnFanDTO>(Messages.UserAlreadyExists);

            model.Email = model.Email.Trim().Replace(" ", "");

            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(model.Password, out passwordHash, out passwordSalt);

            var user = await _userDal.AddAsync(new Entities.Models.User
            {
                GenderId = 3,
                Email = model.Email,
                IsActive = true,
                BirthDate = model.BirthDate,
                CityId = model.CityId,
                CountryId = model.CountryId,
                CoverPictureUrl = null,
                CreationDate = DateTime.Now,
                DisplayName = model.DisplayName,
                IsVerified = false,
                LastLoginAt = DateTime.Now,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                PhoneNumber = model.Phone,
                PrefferedLanguageId = model.PrefferedLanguageId,
                Status = (int)EUserStatus.Approved,
                UserTypeId = (int)EUserType.Fan,
                UserName = model.UserName,
                ProfilePictureUrl = null,
                RefferalCode = null,
                UpdateDate = DateTime.Now,
                IsLocked = false,
            });

            var adminUserRegisterDto = new ReturnFanDTO()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Id = user.Id,
                UserName = user.UserName
            };

            return new SuccessDataResult<ReturnFanDTO>(adminUserRegisterDto,
                Messages.AddingProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(FanRegister), new { data = model });
            return new ErrorDataResult<ReturnFanDTO>(Messages.AddingProcessFailed);
        }
    }
}