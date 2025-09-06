using AutoMapper;
using Business.Abstract;
using Core.Constants;
using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Models;
using System.Linq.Expressions;
namespace Business.Concrete;
public class ReportService : IReportService
{
    private readonly IMapper _mapper;
    readonly ILogger _logger;
    IReportDAL _reportDal;

    public ReportService(IReportDAL reportDal, IMapper mapper, ILogger logger)
    {
        _reportDal = reportDal;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IDataResult<ReportRD>> Create(ReportWD reportWD)
    {
        try
        {
            var data = await _reportDal.AddAsync(_mapper.Map<Report>(reportWD));
            return new SuccessDataResult<ReportRD>(_mapper.Map<ReportRD>(data), Messages.AddingProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Create), new { data = reportWD });
            return new ErrorDataResult<ReportRD>(Messages.AddingProcessFailed);
        }
    }

    public async Task<bool> Delete(long id)
    {
        try
        {
            Report report = await _reportDal.GetAsync(x => x.Id == id);
            if (report != null)
            {
                report.IsActive = false;
                await _reportDal.UpdateAsync(report);
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Delete), new { data = id });
            throw;
        }
    }

    public async Task<IDataResult<List<ReportRD>>> Get(Expression<Func<Entities.Models.Report, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10)
    {
        try
        {
            var reports = await _reportDal.GetPageAsync(filter, x => x.CreationDate, null, includes, (int)pageNumber!, (int)pageSize!);
            return new SuccessDataResult<List<ReportRD>>(_mapper.Map<List<ReportRD>>(reports), Messages.ProcessSuccess, await _reportDal.CountAsync(filter));
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Get), new { data = filter });
            throw new Exception(Messages.GetProcessFailed, ex);
        }
    }

    public async Task<ReportRD?> GetByExpression(Expression<Func<Entities.Models.Report, bool>> filter = null, string[] includes = null)
    {
        try
        {
            var result = await _reportDal.GetAsync(filter, includes);
            if (result != null)
                return _mapper.Map<ReportRD>(result);

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(GetByExpression), new { data = filter });
            throw new Exception(Messages.ProcessFailed, ex);
        }
    }

    public async Task<IDataResult<ReportRD>> Update(ReportWD reportWD)
    {
        try
        {
            await _reportDal.UpdateAsync(_mapper.Map<Report>(reportWD));
            return new SuccessDataResult<ReportRD>(_mapper.Map<ReportRD>(reportWD), Messages.UpdateProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Update), new { data = reportWD });
            return new ErrorDataResult<ReportRD>(Messages.UpdateProcessFailed);
        }
    }

    public async Task<long> Count(Expression<Func<Report, bool>>? filter = null, string[]? includes = null)
    {
        try
        {
            return await _reportDal.CountAsync(filter, includes);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Count), new { data = "" });
            throw;
        }
    }
}