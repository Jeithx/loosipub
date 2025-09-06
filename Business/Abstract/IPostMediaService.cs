using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using System.Linq.Expressions;

namespace Business.Abstract;

public interface IPostMediaService
{
    Task<IDataResult<PostMediaRD>> Create(PostMediaWD model);
    Task<IDataResult<PostMediaRD>> Update(PostMediaWD model);
    Task<bool> Delete(long id);
    Task<IDataResult<List<PostMediaRD>>> Get(Expression<Func<Entities.Models.PostMedia, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10);
    Task<PostMediaRD> GetByExpression(Expression<Func<Entities.Models.PostMedia, bool>> filter = null, string[]? includes = null);
    Task<long> Count(Expression<Func<Entities.Models.PostMedia, bool>>? filter = null, string[]? includes = null);
}