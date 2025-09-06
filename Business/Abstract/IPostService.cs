using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using System.Linq.Expressions;

namespace Business.Abstract;

public interface IPostService
{
    Task<IDataResult<PostRD>> Create(PostWD model);
    Task<IDataResult<PostRD>> Update(PostUpdateWD model);
    Task<bool> Delete(long id, long userId, bool? isJob = false);
    Task<IDataResult<List<PostRD>>> Get(Expression<Func<Entities.Models.Post, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10, long? userId = 0);
    Task<PostRD> GetByExpression(Expression<Func<Entities.Models.Post, bool>> filter = null, string[]? includes = null);
    Task<long> Count(Expression<Func<Entities.Models.Post, bool>>? filter = null, string[]? includes = null);
    Task<IDataResult<List<PostRD>>> GetByRandom(int? pageNumber = 1, int? pageSize = 10, long? userId = 0);
    Task<IDataResult<List<PostRD>>> GetByFollowed(long userId, int? pageNumber = 1, int? pageSize = 10);
}