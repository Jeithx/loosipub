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

public class WalletService : IWalletService
{
    private readonly IMapper _mapper;
    readonly ILogger _logger;
    IWalletDAL _walletDal;

    public WalletService(IWalletDAL walletDal, IMapper mapper, ILogger logger)
    {
        _walletDal = walletDal;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IDataResult<WalletRD>> Create(WalletWD walletWD)
    {
        try
        {
            var dataCheck = await _walletDal.GetAsync(x => x.UserId == walletWD.UserId && x.IsActive);

            if (dataCheck == null)
            {
                var data = await _walletDal.AddAsync(_mapper.Map<Wallet>(walletWD));
                return new SuccessDataResult<WalletRD>(_mapper.Map<WalletRD>(data), Messages.AddingProcessSuccessful);
            }

            return new SuccessDataResult<WalletRD>(_mapper.Map<WalletRD>(walletWD), Messages.AddingProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Create), new { data = walletWD });
            return new ErrorDataResult<WalletRD>(Messages.AddingProcessFailed);
        }
    }

    public async Task<bool> Delete(long id)
    {
        try
        {
            Wallet wallet = await _walletDal.GetAsync(x => x.Id == id);

            await _walletDal.UpdateAsync(wallet);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Delete), new { data = id });
            throw;
        }
    }

    public async Task<IDataResult<List<WalletRD>>> Get(Expression<Func<Entities.Models.Wallet, bool>> filter = null,
        string[]? includes = null, int? pageNumber = 1, int? pageSize = 10)
    {
        try
        {
            var wallets = await _walletDal.GetPageAsync(filter, includes, (int)pageNumber!, (int)pageSize!);
            return new SuccessDataResult<List<WalletRD>>(_mapper.Map<List<WalletRD>>(wallets), Messages.ProcessSuccess,
                await _walletDal.CountAsync(filter));
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Get), new { data = filter });
            throw new Exception(Messages.GetProcessFailed, ex);
        }
    }

    public async Task<WalletRD?> GetByExpression(Expression<Func<Entities.Models.Wallet, bool>> filter = null,
        string[] includes = null)
    {
        try
        {
            var result = await _walletDal.GetAsync(filter, includes);
            if (result != null)
                return _mapper.Map<WalletRD>(result);

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(GetByExpression), new { data = filter });
            throw new Exception(Messages.ProcessFailed, ex);
        }
    }

    public async Task<IDataResult<WalletRD>> Update(WalletWD walletWD)
    {
        try
        {
            await _walletDal.UpdateAsync(_mapper.Map<Wallet>(walletWD));
            return new SuccessDataResult<WalletRD>(_mapper.Map<WalletRD>(walletWD), Messages.UpdateProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Update), new { data = walletWD });
            return new ErrorDataResult<WalletRD>(Messages.UpdateProcessFailed);
        }
    }

    public async Task<long> Count(Expression<Func<Wallet, bool>>? filter = null, string[]? includes = null)
    {
        try
        {
            return await _walletDal.CountAsync(filter, includes);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Count), new { data = "" });
            throw;
        }
    }
}