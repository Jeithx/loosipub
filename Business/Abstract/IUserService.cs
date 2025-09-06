using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using System.Linq.Expressions;

namespace Business.Abstract;

public interface IUserService
{
    Task<IDataResult<UserRD>> Create(UserWD model);
    Task<IDataResult<UserRD>> Update(UserWD model);
    Task<IDataResult<UserRD>> UpdateProfilePhoto(long id, string url);
    Task<IDataResult<UserRD>> UpdateCoverPicture(long id, string url);
    Task<bool> Delete(long id);

    Task<IDataResult<List<UserRD>>> Get(Expression<Func<Entities.Models.User, bool>> filter = null,
        string[]? includes = null, int? pageNumber = 1, int? pageSize = 10);

    Task<UserRD?> GetByExpression(Expression<Func<Entities.Models.User, bool>> filter = null, string[]? includes = null);
    Task<long> Count(Expression<Func<Entities.Models.User, bool>>? filter = null, string[]? includes = null);
    Task<IDataResult<bool>> ChangePassword(ChangePassword model);

    Task<List<UserRD?>> SearchWithFollower(long followedUserId, string username);
    Task<List<UserRD?>> SearchWithFollowed(long userId, string username);
}