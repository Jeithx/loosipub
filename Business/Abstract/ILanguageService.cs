using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using System.Linq.Expressions;

namespace Business.Abstract;

public interface ILanguageService
{
    Task<IDataResult<LanguageRD>> Create(LanguageWD model);
    Task<IDataResult<LanguageRD>> Update(LanguageWD model);
    Task<bool> Delete(long id);
    Task<IDataResult<List<LanguageRD>>> Get(Expression<Func<Entities.Models.Language, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10);
    Task<LanguageRD> GetByExpression(Expression<Func<Entities.Models.Language, bool>> filter = null, string[]? includes = null);
    Task<long> Count(Expression<Func<Entities.Models.Language, bool>>? filter = null, string[]? includes = null);
}