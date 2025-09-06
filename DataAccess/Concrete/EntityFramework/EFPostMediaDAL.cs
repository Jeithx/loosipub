using CorexPack.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Models;

namespace DataAccess.Concrete
{
    public class EFPostMediaDAL : EFEntityRepositoryBaseAsync<PostMedia, LoosipDbContext>, IPostMediaDAL
    {
    }
}
