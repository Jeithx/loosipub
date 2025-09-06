using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using System.Linq.Expressions;

namespace Business.Abstract;

public interface IBlockedUserService
{
    Task<IDataResult<BlockedUserRD>> Create(BlockedUserWD model);
    Task<IDataResult<BlockedUserRD>> Update(BlockedUserWD model);
    Task<bool> Delete(long id, long userId);
    Task<IDataResult<List<BlockedUserRD>>> Get(Expression<Func<Entities.Models.BlockedUser, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10);
    Task<BlockedUserRD> GetByExpression(Expression<Func<Entities.Models.BlockedUser, bool>> filter = null, string[]? includes = null);
    Task<long> Count(Expression<Func<Entities.Models.BlockedUser, bool>>? filter = null, string[]? includes = null);
}