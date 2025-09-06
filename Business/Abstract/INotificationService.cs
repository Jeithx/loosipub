using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using System.Linq.Expressions;

namespace Business.Abstract;

public interface INotificationService
{
    Task<IDataResult<NotificationRD>> Create(NotificationWD model);
    Task<IDataResult<NotificationRD>> Update(NotificationWD model);
    Task<bool> Delete(long id);
    Task<bool> Read(long id, long userId);
    Task<IDataResult<List<NotificationRD>>> Get(Expression<Func<Entities.Models.Notification, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10);
    Task<NotificationRD> GetByExpression(Expression<Func<Entities.Models.Notification, bool>> filter = null, string[]? includes = null);
    Task<long> Count(Expression<Func<Entities.Models.Notification, bool>>? filter = null, string[]? includes = null);
}