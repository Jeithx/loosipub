using Core.Models;
using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using System.Linq.Expressions;
using Entities.Models;

namespace Business.Abstract;

public interface ITagService
{
    Task<IDataResult<TagRD>> Create(TagWD model);
    Task<IDataResult<TagRD>> Update(TagWD model);
    Task<bool> Delete(long id);
    Task<IDataResult<List<TagRD>>> Get(Expression<Func<Entities.Models.Tag, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10);
 Task<TagRD> GetByExpression(Expression<Func<Entities.Models.Tag, bool>> filter = null, string[]? includes = null);
    Task<long> Count(Expression<Func<Entities.Models.Tag, bool>>? filter = null, string[]? includes = null);
}