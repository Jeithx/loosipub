using Core.DataAccess;
using Entities.Models;

namespace DataAccess.Abstract
{
    public interface IWalletLogDAL : IEntityRepositoryAsync<WalletLog>
    {
    }
}