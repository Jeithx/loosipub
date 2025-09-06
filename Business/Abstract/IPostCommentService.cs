using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using System.Linq.Expressions;

namespace Business.Abstract;

public interface IPostCommentService
{
    Task<IDataResult<PostCommentRD>> Create(PostCommentWD model);
    Task<IDataResult<PostCommentRD>> Update(PostCommentWD model);
    Task<bool> Delete(long id, long userId);
    Task<IDataResult<List<PostCommentRD>>> Get(Expression<Func<Entities.Models.PostComment, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10);
    Task<PostCommentRD> GetByExpression(Expression<Func<Entities.Models.PostComment, bool>> filter = null, string[]? includes = null);
    Task<long> Count(Expression<Func<Entities.Models.PostComment, bool>>? filter = null, string[]? includes = null);
}