using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using System.Linq.Expressions;

namespace Business.Abstract;

public interface IPostLikeService
{
    Task<IDataResult<PostLikeRD>> Create(PostLikeWD model);
    Task<IDataResult<PostLikeRD>> Update(PostLikeWD model);
    Task<bool> Delete(long id, long userId);
    Task<IDataResult<List<PostLikeRD>>> Get(Expression<Func<Entities.Models.PostLike, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10, bool? isFollowerCheck = false, long? currentUserId = 0);
    Task<PostLikeRD> GetByExpression(Expression<Func<Entities.Models.PostLike, bool>> filter = null, string[]? includes = null);
    Task<long> Count(Expression<Func<Entities.Models.PostLike, bool>>? filter = null, string[]? includes = null);
}