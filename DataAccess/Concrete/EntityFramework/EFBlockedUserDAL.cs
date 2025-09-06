using CorexPack.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Models;

namespace DataAccess.Concrete
{
    public class EFBlockedUserDAL : EFEntityRepositoryBaseAsync<BlockedUser, LoosipDbContext>, IBlockedUserDAL
    {
    }
}
