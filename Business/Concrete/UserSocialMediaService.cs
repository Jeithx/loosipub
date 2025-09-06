using AutoMapper;
using Business.Abstract;
using Core.Constants;
using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Models;
using System.Linq.Expressions;
namespace Business.Concrete;
public class UserSocialMediaService : IUserSocialMediaService
{
    private readonly IMapper _mapper;
    readonly ILogger _logger;
    IUserSocialMediaDAL _usersocialmediaDal;

    public UserSocialMediaService(IUserSocialMediaDAL usersocialmediaDal, IMapper mapper, ILogger logger)
    {
        _usersocialmediaDal = usersocialmediaDal;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IDataResult<UserSocialMediaRD>> Create(UserSocialMediaWD usersocialmediaWD)
    {
        try
        {
            var data = await _usersocialmediaDal.AddAsync(_mapper.Map<UserSocialMedia>(usersocialmediaWD));
            return new SuccessDataResult<UserSocialMediaRD>(_mapper.Map<UserSocialMediaRD>(data), Messages.AddingProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Create), new { data = usersocialmediaWD });
            return new ErrorDataResult<UserSocialMediaRD>(Messages.AddingProcessFailed);
        }
    }

    public async Task<bool> Delete(long id, long userId)
    {
        try
        {
            UserSocialMedia usersocialmedia = await _usersocialmediaDal.GetAsync(x => x.Id == id && x.UserId == userId && x.IsActive);
            if (usersocialmedia != null)
            {
                usersocialmedia.IsActive = false;
                await _usersocialmediaDal.UpdateAsync(usersocialmedia);
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Delete), new { data = id });
            throw;
        }
    }

    public async Task<IDataResult<List<UserSocialMediaRD>>> Get(Expression<Func<Entities.Models.UserSocialMedia, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10)
    {
        try
        {
            var usersocialmedias = await _usersocialmediaDal.GetPageAsync(filter, includes, (int)pageNumber!, (int)pageSize!);
            return new SuccessDataResult<List<UserSocialMediaRD>>(_mapper.Map<List<UserSocialMediaRD>>(usersocialmedias), Messages.ProcessSuccess, await _usersocialmediaDal.CountAsync(filter));
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Get), new { data = filter });
            throw new Exception(Messages.GetProcessFailed, ex);
        }
    }

    public async Task<UserSocialMediaRD?> GetByExpression(Expression<Func<Entities.Models.UserSocialMedia, bool>> filter = null, string[] includes = null)
    {
        try
        {
            var result = await _usersocialmediaDal.GetAsync(filter, includes);
            if (result != null)
                return _mapper.Map<UserSocialMediaRD>(result);

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(GetByExpression), new { data = filter });
            throw new Exception(Messages.ProcessFailed, ex);
        }
    }

    public async Task<IDataResult<UserSocialMediaRD>> Update(UserSocialMediaWD usersocialmediaWD)
    {
        try
        {
            await _usersocialmediaDal.UpdateAsync(_mapper.Map<UserSocialMedia>(usersocialmediaWD));
            return new SuccessDataResult<UserSocialMediaRD>(_mapper.Map<UserSocialMediaRD>(usersocialmediaWD), Messages.UpdateProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Update), new { data = usersocialmediaWD });
            return new ErrorDataResult<UserSocialMediaRD>(Messages.UpdateProcessFailed);
        }
    }

    public async Task<long> Count(Expression<Func<UserSocialMedia, bool>>? filter = null, string[]? includes = null)
    {
        try
        {
            return await _usersocialmediaDal.CountAsync(filter, includes);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Count), new { data = "" });
            throw;
        }
    }
}