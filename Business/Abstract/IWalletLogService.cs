using Core.Models;
using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using System.Linq.Expressions;
using Entities.Models;

namespace Business.Abstract;

public interface IWalletLogService
{
    Task<IDataResult<WalletLogRD>> Create(WalletLogWD model);
    Task<IDataResult<WalletLogRD>> Update(WalletLogWD model);
    Task<bool> Delete(long id);
    Task<IDataResult<List<WalletLogRD>>> Get(Expression<Func<Entities.Models.WalletLog, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10);
 Task<WalletLogRD> GetByExpression(Expression<Func<Entities.Models.WalletLog, bool>> filter = null, string[]? includes = null);
    Task<long> Count(Expression<Func<Entities.Models.WalletLog, bool>>? filter = null, string[]? includes = null);
}