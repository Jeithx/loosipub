using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using System.Linq.Expressions;

namespace Business.Abstract;

public interface IPostTagService
{
    Task<IDataResult<PostTagRD>> Create(PostTagWD model);
    Task<IDataResult<PostTagRD>> Update(PostTagWD model);
    Task<bool> Delete(long postId, long tagId);
    Task<IDataResult<List<PostTagRD>>> Get(Expression<Func<Entities.Models.PostTag, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10);
    Task<PostTagRD> GetByExpression(Expression<Func<Entities.Models.PostTag, bool>> filter = null, string[]? includes = null);
    Task<long> Count(Expression<Func<Entities.Models.PostTag, bool>>? filter = null, string[]? includes = null);
}