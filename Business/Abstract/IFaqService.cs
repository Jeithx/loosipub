using Core.Models;
using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using System.Linq.Expressions;
using Entities.Models;

namespace Business.Abstract;

public interface IFaqService
{
    Task<IDataResult<FaqRD>> Create(FaqWD model);
    Task<IDataResult<FaqRD>> Update(FaqWD model);
    Task<bool> Delete(long id);
    Task<IDataResult<List<FaqRD>>> Get(Expression<Func<Entities.Models.Faq, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10);
 Task<FaqRD> GetByExpression(Expression<Func<Entities.Models.Faq, bool>> filter = null, string[]? includes = null);
    Task<long> Count(Expression<Func<Entities.Models.Faq, bool>>? filter = null, string[]? includes = null);
}