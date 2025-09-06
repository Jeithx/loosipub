using CorexPack.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Models;

namespace DataAccess.Concrete
{
    public class EFPostTagDAL : EFEntityRepositoryBaseAsync<PostTag, LoosipDbContext>, IPostTagDAL
    {
    }
}
