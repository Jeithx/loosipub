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
public class WalletLogService : IWalletLogService
{
    private readonly IMapper _mapper;
    readonly ILogger _logger;
    IWalletLogDAL _walletlogDal;

    public WalletLogService(IWalletLogDAL walletlogDal, IMapper mapper, ILogger logger)
    {
        _walletlogDal = walletlogDal;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IDataResult<WalletLogRD>> Create(WalletLogWD walletlogWD)
    {
        try
        {
            var data = await _walletlogDal.AddAsync(_mapper.Map<WalletLog>(walletlogWD));
            return new SuccessDataResult<WalletLogRD>(_mapper.Map<WalletLogRD>(data), Messages.AddingProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Create), new { data = walletlogWD });
            return new ErrorDataResult<WalletLogRD>(Messages.AddingProcessFailed);
        }
    }

    public async Task<bool> Delete(long id)
    {
        try
        {
            WalletLog walletlog = await _walletlogDal.GetAsync(x => x.Id == id);

            await _walletlogDal.UpdateAsync(walletlog);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Delete), new { data = id });
            throw;
        }
    }

    public async Task<IDataResult<List<WalletLogRD>>> Get(Expression<Func<Entities.Models.WalletLog, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10)
    {
        try
        {
            var walletlogs = await _walletlogDal.GetPageAsync(filter, x => x.CreationDate, null, includes, (int)pageNumber!, (int)pageSize!);
            return new SuccessDataResult<List<WalletLogRD>>(_mapper.Map<List<WalletLogRD>>(walletlogs), Messages.ProcessSuccess, await _walletlogDal.CountAsync(filter));
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Get), new { data = filter });
            throw new Exception(Messages.GetProcessFailed, ex);
        }
    }

    public async Task<WalletLogRD?> GetByExpression(Expression<Func<Entities.Models.WalletLog, bool>> filter = null, string[] includes = null)
    {
        try
        {
            var result = await _walletlogDal.GetAsync(filter, includes);
            if (result != null)
                return _mapper.Map<WalletLogRD>(result);

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(GetByExpression), new { data = filter });
            throw new Exception(Messages.ProcessFailed, ex);
        }
    }

    public async Task<IDataResult<WalletLogRD>> Update(WalletLogWD walletlogWD)
    {
        try
        {
            await _walletlogDal.UpdateAsync(_mapper.Map<WalletLog>(walletlogWD));
            return new SuccessDataResult<WalletLogRD>(_mapper.Map<WalletLogRD>(walletlogWD), Messages.UpdateProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Update), new { data = walletlogWD });
            return new ErrorDataResult<WalletLogRD>(Messages.UpdateProcessFailed);
        }
    }

    public async Task<long> Count(Expression<Func<WalletLog, bool>>? filter = null, string[]? includes = null)
    {
        try
        {
            return await _walletlogDal.CountAsync(filter, includes);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Count), new { data = "" });
            throw;
        }
    }
}