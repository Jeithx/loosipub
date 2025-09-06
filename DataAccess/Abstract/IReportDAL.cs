using Core.DataAccess;
using Entities.Models;

namespace DataAccess.Abstract
{
    public interface IReportDAL : IEntityRepositoryAsync<Report>
    {
    }
}