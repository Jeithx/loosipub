using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using System.Linq.Expressions;

namespace Business.Abstract;

public interface ICallPaymentService
{
    Task<IDataResult<CallPaymentRD>> Create(CallPaymentWD model);
    Task<IDataResult<CallPaymentRD>> Update(CallPaymentWD model);
    Task<bool> Delete(long id, long userId);
    Task<IDataResult<List<CallPaymentRD>>> Get(Expression<Func<Entities.Models.CallPayment, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10);
    Task<CallPaymentRD> GetByExpression(Expression<Func<Entities.Models.CallPayment, bool>> filter = null, string[]? includes = null);
    Task<long> Count(Expression<Func<Entities.Models.CallPayment, bool>>? filter = null, string[]? includes = null);
}