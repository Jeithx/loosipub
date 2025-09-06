using Core.DataAccess;
using Entities.Models;

namespace DataAccess.Abstract
{
    public interface IFaqDAL : IEntityRepositoryAsync<Faq>
    {
    }
}