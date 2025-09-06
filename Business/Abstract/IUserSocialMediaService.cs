using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using System.Linq.Expressions;

namespace Business.Abstract;

public interface IUserSocialMediaService
{
    Task<IDataResult<UserSocialMediaRD>> Create(UserSocialMediaWD model);
    Task<IDataResult<UserSocialMediaRD>> Update(UserSocialMediaWD model);
    Task<bool> Delete(long id, long userId);
    Task<IDataResult<List<UserSocialMediaRD>>> Get(Expression<Func<Entities.Models.UserSocialMedia, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10);
    Task<UserSocialMediaRD> GetByExpression(Expression<Func<Entities.Models.UserSocialMedia, bool>> filter = null, string[]? includes = null);
    Task<long> Count(Expression<Func<Entities.Models.UserSocialMedia, bool>>? filter = null, string[]? includes = null);
}