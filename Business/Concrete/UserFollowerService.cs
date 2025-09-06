using AutoMapper;
using Business.Abstract;
using Core.Constants;
using Core.Enums;
using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Models;
using System.Linq.Expressions;
namespace Business.Concrete;
public class UserFollowerService : IUserFollowerService
{
    private readonly IMapper _mapper;
    readonly ILogger _logger;
    IUserFollowerDAL _userfollowerDal;
    readonly INotificationService _notificationService;

    public UserFollowerService(IUserFollowerDAL userfollowerDal, IMapper mapper, ILogger logger, INotificationService notificationService)
    {
        _userfollowerDal = userfollowerDal;
        _mapper = mapper;
        _logger = logger;
        _notificationService = notificationService;
    }

    public async Task<IDataResult<UserFollowerRD>> Create(UserFollowerWD userfollowerWD)
    {
        try
        {
            var dataCheck = await _userfollowerDal.GetAsync(x => x.FollowerId == userfollowerWD.FollowerId && x.FollowedId == userfollowerWD.FollowedId && x.IsActive);
            if (dataCheck == null)
            {
                var data = await _userfollowerDal.AddAsync(_mapper.Map<UserFollower>(userfollowerWD));
                if (data != null)
                {
                    await _notificationService.Create(new NotificationWD
                    {
                        UserId = data.FollowedId,
                        Content = $"started following you.",
                        CreationDate = DateTime.Now,
                        IsActive = true,
                        IsRead = false,
                        Type = (int)ENotificationType.NewFollower,
                        UpdateDate = DateTime.Now
                    });
                }
                return new SuccessDataResult<UserFollowerRD>(_mapper.Map<UserFollowerRD>(data), Messages.AddingProcessSuccessful);
            }

            return new SuccessDataResult<UserFollowerRD>(_mapper.Map<UserFollowerRD>(userfollowerWD), Messages.AddingProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Create), new { data = userfollowerWD });
            return new ErrorDataResult<UserFollowerRD>(Messages.AddingProcessFailed);
        }
    }

    public async Task<bool> Delete(long id, long userId)
    {
        try
        {
            var userfollower = await _userfollowerDal.GetAsync(x => x.FollowedId == id && x.IsActive && x.FollowerId == userId);
            if (userfollower != null)
            {
                userfollower.IsActive = false;
                await _userfollowerDal.UpdateAsync(userfollower);
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Delete), new { data = id });
            throw;
        }
    }

    public async Task<IDataResult<List<UserFollowerRD>>> Get(Expression<Func<Entities.Models.UserFollower, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10)
    {
        try
        {
            var userfollowers = await _userfollowerDal.GetPageAsync(filter, x => x.CreationDate, null, includes, (int)pageNumber!, (int)pageSize!);
            return new SuccessDataResult<List<UserFollowerRD>>(_mapper.Map<List<UserFollowerRD>>(userfollowers), Messages.ProcessSuccess, await _userfollowerDal.CountAsync(filter));
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Get), new { data = filter });
            throw new Exception(Messages.GetProcessFailed, ex);
        }
    }

    public async Task<UserFollowerRD?> GetByExpression(Expression<Func<Entities.Models.UserFollower, bool>> filter = null, string[] includes = null)
    {
        try
        {
            var result = await _userfollowerDal.GetAsync(filter, includes);
            if (result != null)
                return _mapper.Map<UserFollowerRD>(result);

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(GetByExpression), new { data = filter });
            throw new Exception(Messages.ProcessFailed, ex);
        }
    }

    public async Task<IDataResult<UserFollowerRD>> Update(UserFollowerWD userfollowerWD)
    {
        try
        {
            await _userfollowerDal.UpdateAsync(_mapper.Map<UserFollower>(userfollowerWD));
            return new SuccessDataResult<UserFollowerRD>(_mapper.Map<UserFollowerRD>(userfollowerWD), Messages.UpdateProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Update), new { data = userfollowerWD });
            return new ErrorDataResult<UserFollowerRD>(Messages.UpdateProcessFailed);
        }
    }

    public async Task<long> Count(Expression<Func<UserFollower, bool>>? filter = null, string[]? includes = null)
    {
        try
        {
            return await _userfollowerDal.CountAsync(filter, includes);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Count), new { data = "" });
            throw;
        }
    }
}