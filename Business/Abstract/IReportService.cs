using Core.Models;
using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using System.Linq.Expressions;
using Entities.Models;

namespace Business.Abstract;

public interface IReportService
{
    Task<IDataResult<ReportRD>> Create(ReportWD model);
    Task<IDataResult<ReportRD>> Update(ReportWD model);
    Task<bool> Delete(long id);
    Task<IDataResult<List<ReportRD>>> Get(Expression<Func<Entities.Models.Report, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10);
 Task<ReportRD> GetByExpression(Expression<Func<Entities.Models.Report, bool>> filter = null, string[]? includes = null);
    Task<long> Count(Expression<Func<Entities.Models.Report, bool>>? filter = null, string[]? includes = null);
}