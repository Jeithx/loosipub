using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using System.Linq.Expressions;

namespace Business.Abstract;

public interface ICityService
{
    Task<IDataResult<CityRD>> Create(CityWD model);
    Task<IDataResult<CityRD>> Update(CityWD model);
    Task<bool> Delete(long id);
    Task<IDataResult<List<CityRD>>> Get(Expression<Func<Entities.Models.City, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10, bool notPagination = false);
    Task<CityRD> GetByExpression(Expression<Func<Entities.Models.City, bool>> filter = null, string[]? includes = null);
    Task<long> Count(Expression<Func<Entities.Models.City, bool>>? filter = null, string[]? includes = null);
}