using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using System.Linq.Expressions;

namespace Business.Abstract;

public interface IGenderService
{
    Task<IDataResult<GenderRD>> Create(GenderWD model);
    Task<IDataResult<GenderRD>> Update(GenderWD model);
    Task<bool> Delete(long id);
    Task<IDataResult<List<GenderRD>>> Get(Expression<Func<Entities.Models.Gender, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10);
    Task<GenderRD> GetByExpression(Expression<Func<Entities.Models.Gender, bool>> filter = null, string[]? includes = null);
    Task<long> Count(Expression<Func<Entities.Models.Gender, bool>>? filter = null, string[]? includes = null);
}