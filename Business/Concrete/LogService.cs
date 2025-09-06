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
public class LogService : ILogService
{
    private readonly IMapper _mapper;
    readonly ILogger _logger;
    ILogDAL _logDal;

    public LogService(ILogDAL logDal, IMapper mapper, ILogger logger)
    {
        _logDal = logDal;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IDataResult<LogRD>> Create(LogWD logWD)
    {
        try
        {
            var data = await _logDal.AddAsync(_mapper.Map<Log>(logWD));
            return new SuccessDataResult<LogRD>(_mapper.Map<LogRD>(data), Messages.AddingProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Create), new { data = logWD });
            return new ErrorDataResult<LogRD>(Messages.AddingProcessFailed);
        }
    }

    public async Task<bool> Delete(long id)
    {
        try
        {
            Log log = await _logDal.GetAsync(x => x.Id == id);

            await _logDal.UpdateAsync(log);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Delete), new { data = id });
            throw;
        }
    }

    public async Task<IDataResult<List<LogRD>>> Get(Expression<Func<Entities.Models.Log, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10)
    {
        try
        {
            var logs = await _logDal.GetPageAsync(filter, includes, (int)pageNumber!, (int)pageSize!);
            return new SuccessDataResult<List<LogRD>>(_mapper.Map<List<LogRD>>(logs), Messages.ProcessSuccess, await _logDal.CountAsync(filter));
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Get), new { data = filter });
            throw new Exception(Messages.GetProcessFailed, ex);
        }
    }

    public async Task<LogRD?> GetByExpression(Expression<Func<Entities.Models.Log, bool>> filter = null, string[] includes = null)
    {
        try
        {
            var result = await _logDal.GetAsync(filter, includes);
            if (result != null)
                return _mapper.Map<LogRD>(result);

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(GetByExpression), new { data = filter });
            throw new Exception(Messages.ProcessFailed, ex);
        }
    }

    public async Task<IDataResult<LogRD>> Update(LogWD logWD)
    {
        try
        {
            await _logDal.UpdateAsync(_mapper.Map<Log>(logWD));
            return new SuccessDataResult<LogRD>(_mapper.Map<LogRD>(logWD), Messages.UpdateProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Update), new { data = logWD });
            return new ErrorDataResult<LogRD>(Messages.UpdateProcessFailed);
        }
    }

    public async Task<long> Count(Expression<Func<Log, bool>>? filter = null, string[]? includes = null)
    {
        try
        {
            return await _logDal.CountAsync(filter, includes);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Count), new { data = "" });
            throw;
        }
    }
}