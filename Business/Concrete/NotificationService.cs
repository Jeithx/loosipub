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
public class NotificationService : INotificationService
{
    private readonly IMapper _mapper;
    readonly ILogger _logger;
    INotificationDAL _notificationDal;

    public NotificationService(INotificationDAL notificationDal, IMapper mapper, ILogger logger)
    {
        _notificationDal = notificationDal;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IDataResult<NotificationRD>> Create(NotificationWD notificationWD)
    {
        try
        {
            notificationWD.CreationDate = DateTime.Now;
            notificationWD.IsActive = true;
            notificationWD.IsRead = false;
            var data = await _notificationDal.AddAsync(_mapper.Map<Notification>(notificationWD));
            return new SuccessDataResult<NotificationRD>(_mapper.Map<NotificationRD>(data), Messages.AddingProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Create), new { data = notificationWD });
            return new ErrorDataResult<NotificationRD>(Messages.AddingProcessFailed);
        }
    }

    public async Task<bool> Read(long id, long userId)
    {
        try
        {
            var notification = await _notificationDal.GetAsync(x => x.Id == id && x.IsActive && x.UserId == userId);
            if (notification != null)
            {
                notification.IsRead = true;
                await _notificationDal.UpdateAsync(notification);
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Delete), new { data = id });
            throw;
        }
    }

    public async Task<bool> Delete(long id)
    {
        try
        {
            Notification notification = await _notificationDal.GetAsync(x => x.Id == id);

            await _notificationDal.UpdateAsync(notification);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Delete), new { data = id });
            throw;
        }
    }

    public async Task<IDataResult<List<NotificationRD>>> Get(Expression<Func<Entities.Models.Notification, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10)
    {
        try
        {
            var notifications = await _notificationDal.GetPageAsync(filter, x => x.CreationDate, null, includes, (int)pageNumber!, (int)pageSize!);
            return new SuccessDataResult<List<NotificationRD>>(_mapper.Map<List<NotificationRD>>(notifications), Messages.ProcessSuccess, await _notificationDal.CountAsync(filter));
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Get), new { data = filter });
            throw new Exception(Messages.GetProcessFailed, ex);
        }
    }

    public async Task<NotificationRD?> GetByExpression(Expression<Func<Entities.Models.Notification, bool>> filter = null, string[] includes = null)
    {
        try
        {
            var result = await _notificationDal.GetAsync(filter, includes);
            if (result != null)
                return _mapper.Map<NotificationRD>(result);

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(GetByExpression), new { data = filter });
            throw new Exception(Messages.ProcessFailed, ex);
        }
    }

    public async Task<IDataResult<NotificationRD>> Update(NotificationWD notificationWD)
    {
        try
        {
            await _notificationDal.UpdateAsync(_mapper.Map<Notification>(notificationWD));
            return new SuccessDataResult<NotificationRD>(_mapper.Map<NotificationRD>(notificationWD), Messages.UpdateProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Update), new { data = notificationWD });
            return new ErrorDataResult<NotificationRD>(Messages.UpdateProcessFailed);
        }
    }

    public async Task<long> Count(Expression<Func<Notification, bool>>? filter = null, string[]? includes = null)
    {
        try
        {
            return await _notificationDal.CountAsync(filter, includes);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Count), new { data = "" });
            throw;
        }
    }
}