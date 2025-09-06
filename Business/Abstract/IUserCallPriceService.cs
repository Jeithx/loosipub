using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using System.Linq.Expressions;

namespace Business.Abstract;

public interface IUserCallPriceService
{
    Task<IDataResult<UserCallPriceRD>> Create(UserCallPriceWD model);
    Task<IDataResult<UserCallPriceRD>> Update(UserCallPriceWD model);
    Task<bool> Delete(long id, long userId);
    Task<IDataResult<List<UserCallPriceRD>>> Get(Expression<Func<Entities.Models.UserCallPrice, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10);
    Task<UserCallPriceRD> GetByExpression(Expression<Func<Entities.Models.UserCallPrice, bool>> filter = null, string[]? includes = null);
    Task<long> Count(Expression<Func<Entities.Models.UserCallPrice, bool>>? filter = null, string[]? includes = null);
}