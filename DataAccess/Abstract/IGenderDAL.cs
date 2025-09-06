using Core.DataAccess;
using Entities.Models;

namespace DataAccess.Abstract
{
    public interface IGenderDAL : IEntityRepositoryAsync<Gender>
    {
    }
}