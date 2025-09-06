using Core.DataAccess;
using Entities.Models;

namespace DataAccess.Abstract
{
    public interface ICountryDAL : IEntityRepositoryAsync<Country>
    {
    }
}