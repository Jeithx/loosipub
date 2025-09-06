using CorexPack.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Models;

namespace DataAccess.Concrete
{
    public class EFCountryDAL : EFEntityRepositoryBaseAsync<Country, LoosipDbContext>, ICountryDAL
    {
    }
}
