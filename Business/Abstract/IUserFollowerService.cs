using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using System.Linq.Expressions;

namespace Business.Abstract;

public interface IUserFollowerService
{
    Task<IDataResult<UserFollowerRD>> Create(UserFollowerWD model);
    Task<IDataResult<UserFollowerRD>> Update(UserFollowerWD model);
    Task<bool> Delete(long id, long userId);
    Task<IDataResult<List<UserFollowerRD>>> Get(Expression<Func<Entities.Models.UserFollower, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10);
    Task<UserFollowerRD> GetByExpression(Expression<Func<Entities.Models.UserFollower, bool>> filter = null, string[]? includes = null);
    Task<long> Count(Expression<Func<Entities.Models.UserFollower, bool>>? filter = null, string[]? includes = null);
}