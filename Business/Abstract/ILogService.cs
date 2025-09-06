using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using System.Linq.Expressions;

namespace Business.Abstract;

public interface ILogService
{
    Task<IDataResult<LogRD>> Create(LogWD model);
    Task<IDataResult<LogRD>> Update(LogWD model);
    Task<bool> Delete(long id);
    Task<IDataResult<List<LogRD>>> Get(Expression<Func<Entities.Models.Log, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10);
    Task<LogRD> GetByExpression(Expression<Func<Entities.Models.Log, bool>> filter = null, string[]? includes = null);
    Task<long> Count(Expression<Func<Entities.Models.Log, bool>>? filter = null, string[]? includes = null);
}