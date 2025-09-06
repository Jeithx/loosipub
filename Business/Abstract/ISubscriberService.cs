using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using System.Linq.Expressions;

namespace Business.Abstract;

public interface ISubscriberService
{
    Task<IDataResult<SubscriberRD>> Create(SubscriberWD model);
    Task<IDataResult<SubscriberRD>> Update(SubscriberWD model);
    Task<bool> Delete(long id, long userId);
    Task<IDataResult<List<SubscriberRD>>> Get(Expression<Func<Entities.Models.Subscriber, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10);
    Task<SubscriberRD> GetByExpression(Expression<Func<Entities.Models.Subscriber, bool>> filter = null, string[]? includes = null);
    Task<long> Count(Expression<Func<Entities.Models.Subscriber, bool>>? filter = null, string[]? includes = null);
}