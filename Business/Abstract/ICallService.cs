using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using System.Linq.Expressions;

namespace Business.Abstract;

public interface ICallService
{
    Task<IDataResult<CallRD>> Create(CallWD model);
    Task<IDataResult<CallRD>> Update(CallWD model);
    Task<bool> Delete(long id, long userId);
    Task<IDataResult<List<CallRD>>> Get(Expression<Func<Entities.Models.Call, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10);
    Task<CallRD> GetByExpression(Expression<Func<Entities.Models.Call, bool>> filter = null, string[]? includes = null);
    Task<long> Count(Expression<Func<Entities.Models.Call, bool>>? filter = null, string[]? includes = null);
}