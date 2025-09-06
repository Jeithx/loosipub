using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using System.Linq.Expressions;

namespace Business.Abstract;

public interface ICountryService
{
    Task<IDataResult<CountryRD>> Create(CountryWD model);
    Task<IDataResult<CountryRD>> Update(CountryWD model);
    Task<bool> Delete(long id);
    Task<IDataResult<List<CountryRD>>> Get(Expression<Func<Entities.Models.Country, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10,bool notPagination= false);
    Task<CountryRD> GetByExpression(Expression<Func<Entities.Models.Country, bool>> filter = null, string[]? includes = null);
    Task<long> Count(Expression<Func<Entities.Models.Country, bool>>? filter = null, string[]? includes = null);
}