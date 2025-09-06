using Core.DataAccess;
using Entities.Models;

namespace DataAccess.Abstract
{
    public interface IUserDAL : IEntityRepositoryAsync<User>
    {
    }
}