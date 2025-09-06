using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using System.Linq.Expressions;

namespace Business.Abstract;

public interface IUserLoginAttemptService
{
    Task<IDataResult<UserLoginAttemptRD>> Create(UserLoginAttemptWD model);
    Task<IDataResult<UserLoginAttemptRD>> Update(UserLoginAttemptWD model);
    Task<IDataResult<List<UserLoginAttemptRD>>> Get(Expression<Func<Entities.Models.UserLoginAttempt, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10);
    Task<UserLoginAttemptRD> GetByExpression(Expression<Func<Entities.Models.UserLoginAttempt, bool>> filter = null, string[]? includes = null);
    Task<long> Count(Expression<Func<Entities.Models.UserLoginAttempt, bool>>? filter = null, string[]? includes = null);
}