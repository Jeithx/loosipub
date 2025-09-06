using Core.Models;
using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using System.Linq.Expressions;
using Entities.Models;

namespace Business.Abstract;

public interface IWalletService
{
    Task<IDataResult<WalletRD>> Create(WalletWD model);
    Task<IDataResult<WalletRD>> Update(WalletWD model);
    Task<bool> Delete(long id);
    Task<IDataResult<List<WalletRD>>> Get(Expression<Func<Entities.Models.Wallet, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10);
 Task<WalletRD> GetByExpression(Expression<Func<Entities.Models.Wallet, bool>> filter = null, string[]? includes = null);
    Task<long> Count(Expression<Func<Entities.Models.Wallet, bool>>? filter = null, string[]? includes = null);
}