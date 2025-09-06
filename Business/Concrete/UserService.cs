using AutoMapper;
using Business.Abstract;
using Core.Constants;
using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using DataAccess.Abstract;
using Entities.Models;
using System.Linq.Expressions;

namespace Business.Concrete;
public class UserService : IUserService
{
    private readonly IMapper _mapper;
    readonly ILogger _logger;
    IUserDAL _userDal;
    readonly IUserFollowerDAL _userFollowerDal;

    public UserService(IUserDAL userDal, IMapper mapper, ILogger logger, IUserFollowerDAL userFollowerDal)
    {
        _userDal = userDal;
        _mapper = mapper;
        _logger = logger;
        _userFollowerDal = userFollowerDal;
    }

    public async Task<IDataResult<bool>> ChangePassword(ChangePassword model)
    {
        try
        {
            if (model.NewPassword != model.NewPasswordRepeat)
                return new ErrorDataResult<bool>(false, Messages.PasswordWrong);

            var user = await _userDal.GetAsync(x => x.IsActive == true && x.Id == model.Id);
            if (!HashingHelper.VerifyPasswordHash(model.Password, user.PasswordHash, user.PasswordSalt))
                return new ErrorDataResult<bool>(false, Messages.PasswordWrong);

            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(model.NewPassword, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.UpdateDate = DateTime.Now;
            await _userDal.UpdateAsync(user);

            return new SuccessDataResult<bool>(true, Messages.AddingProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(ChangePassword), new { data = model });
            return new ErrorDataResult<bool>(false, Messages.AddingProcessFailed);
        }
    }

    public async Task<IDataResult<UserRD>> Create(UserWD userWD)
    {
        try
        {
            var data = await _userDal.AddAsync(_mapper.Map<User>(userWD));
            return new SuccessDataResult<UserRD>(_mapper.Map<UserRD>(data), Messages.AddingProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Create), new { data = userWD });
            return new ErrorDataResult<UserRD>(Messages.AddingProcessFailed);
        }
    }

    public async Task<bool> Delete(long id)
    {
        try
        {
            User user = await _userDal.GetAsync(x => x.Id == id);

            await _userDal.UpdateAsync(user);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Delete), new { data = id });
            throw;
        }
    }

    public async Task<IDataResult<List<UserRD>>> Get(Expression<Func<Entities.Models.User, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10)
    {
        try
        {
            var users = await _userDal.GetPageAsync(filter, includes, (int)pageNumber!, (int)pageSize!);
            return new SuccessDataResult<List<UserRD>>(_mapper.Map<List<UserRD>>(users), Messages.ProcessSuccess, await _userDal.CountAsync(filter));
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Get), new { data = filter });
            throw new Exception(Messages.GetProcessFailed, ex);
        }
    }

    public async Task<UserRD?> GetByExpression(Expression<Func<Entities.Models.User, bool>> filter = null, string[] includes = null)
    {
        try
        {
            var result = await _userDal.GetAsync(filter, includes);
            if (result != null)
                return _mapper.Map<UserRD>(result);

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(GetByExpression), new { data = filter });
            throw new Exception(Messages.ProcessFailed, ex);
        }
    }

    //Takip ettikleri içinde arama yapar
    public async Task<List<UserRD?>> SearchWithFollowed(long userId, string username)
    {
        try
        {
            var result = await _userFollowerDal.GetListAsync(x => x.FollowerId == userId && x.Followed.UserName.Contains(username.Trim()) && x.IsActive, new[] { "Followed" });
            if (result != null)
            {
                var list = result.Select(x => x.Followed).ToList();

                return _mapper.Map<List<UserRD?>>(list);
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(GetByExpression), new { data = userId });
            throw new Exception(Messages.ProcessFailed, ex);
        }
    }

    //Takip edenler içinde arama yapar
    public async Task<List<UserRD?>> SearchWithFollower(long followedUserId, string username)
    {
        try
        {
            var result = await _userFollowerDal.GetListAsync(x => x.FollowedId == followedUserId && x.Follower.UserName.Contains(username.Trim()) && x.IsActive, new[] { "Follower" });
            if (result != null)
            {
                var list = result.Select(x => x.Follower).ToList();

                return _mapper.Map<List<UserRD?>>(list);
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(GetByExpression), new { data = followedUserId });
            throw new Exception(Messages.ProcessFailed, ex);
        }
    }

    public async Task<IDataResult<UserRD>> Update(UserWD userWD)
    {
        try
        {
            var user = await _userDal.GetAsync(x => x.Id == userWD.Id, null);
            userWD.PasswordHash = user.PasswordHash;
            userWD.PasswordSalt = user.PasswordSalt;
            userWD.UpdateDate = DateTime.Now;
            userWD.IsActive = true;
            userWD.IsVerified = user.IsVerified;
            userWD.GenderId = user.GenderId;
            userWD.UserTypeId = user.UserTypeId;
            userWD.Status = user.Status;
            userWD.CreationDate = user.CreationDate;
            userWD.LastLoginAt = user.LastLoginAt;
            userWD.CoverPictureUrl = user.CoverPictureUrl;
            userWD.ProfilePictureUrl = user.ProfilePictureUrl;

            if (!string.IsNullOrEmpty(userWD.UserName))
            {
                var usernameCheck = await _userDal.GetAsync(x => x.UserName == userWD.UserName && x.Id != userWD.Id);
                if (usernameCheck == null)
                {
                    userWD.UserName = userWD.UserName.Trim();
                }
            }

            if (!string.IsNullOrEmpty(userWD.Email))
            {
                var emailCheck = await _userDal.GetAsync(x => x.Email == userWD.Email && x.Id != userWD.Id);
                if (emailCheck == null)
                {
                    userWD.Email = userWD.Email.Trim();
                }
            }

            if (!string.IsNullOrEmpty(userWD.PhoneNumber))
            {
                var phoneNumberCheck = await _userDal.GetAsync(x => x.PhoneNumber == userWD.PhoneNumber && x.Id != userWD.Id);
                if (phoneNumberCheck == null)
                {
                    userWD.PhoneNumber = userWD.PhoneNumber.Trim();
                }
            }

            await _userDal.UpdateAsync(_mapper.Map<User>(userWD));
            return new SuccessDataResult<UserRD>(_mapper.Map<UserRD>(userWD), Messages.UpdateProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Update), new { data = userWD });
            return new ErrorDataResult<UserRD>(Messages.UpdateProcessFailed);
        }
    }

    public async Task<long> Count(Expression<Func<User, bool>>? filter = null, string[]? includes = null)
    {
        try
        {
            return await _userDal.CountAsync(filter, includes);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Count), new { data = "" });
            throw;
        }
    }

    public async Task<IDataResult<UserRD>> UpdateProfilePhoto(long id, string url)
    {
        try
        {
            var user = await _userDal.GetAsync(x => x.Id == id && x.IsActive);
            user.ProfilePictureUrl = url;
            user.UpdateDate = DateTime.Now;
            await _userDal.UpdateAsync(user);
            return new SuccessDataResult<UserRD>(_mapper.Map<UserRD>(user), Messages.UpdateProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Update), new { data = id });
            return new ErrorDataResult<UserRD>(Messages.UpdateProcessFailed);
        }
    }

    public async Task<IDataResult<UserRD>> UpdateCoverPicture(long id, string url)
    {
        try
        {
            var user = await _userDal.GetAsync(x => x.Id == id && x.IsActive);
            user.CoverPictureUrl = url;
            user.UpdateDate = DateTime.Now;
            await _userDal.UpdateAsync(user);
            return new SuccessDataResult<UserRD>(_mapper.Map<UserRD>(user), Messages.UpdateProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Update), new { data = id });
            return new ErrorDataResult<UserRD>(Messages.UpdateProcessFailed);
        }
    }
}