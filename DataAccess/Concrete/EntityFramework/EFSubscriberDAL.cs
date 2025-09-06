using CorexPack.DataAccess.EntityFramework;
using Entities.Models;
using DataAccess.Abstract;
using Entities.Models;

namespace DataAccess.Concrete{
    public class EFSubscriberDAL : EFEntityRepositoryBaseAsync<Subscriber, LoosipDbContext>, ISubscriberDAL
    {
    }
}
