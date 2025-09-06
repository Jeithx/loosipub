using AutoMapper;
using Business.Abstract;
using Core.Constants;
using Core.Enums;
using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Models;
using System.Linq.Expressions;
namespace Business.Concrete;
public class CallService : ICallService
{
    private readonly IMapper _mapper;
    readonly ILogger _logger;
    readonly ICallDAL _callDal;
    readonly ICallPaymentDAL _callPaymentDal;
    readonly IUserCallPriceDAL _userCallPriceDal;
    readonly IWalletDAL _walletDal;
    readonly IWalletLogDAL _walletLogDAL;

    public CallService(ICallDAL callDal, IMapper mapper, ILogger logger, ICallPaymentDAL callPaymentDal, IUserCallPriceDAL userCallPriceDal, IWalletDAL walletDal, IWalletLogDAL walletLogDAL)
    {
        _callDal = callDal;
        _mapper = mapper;
        _logger = logger;
        _callPaymentDal = callPaymentDal;
        _userCallPriceDal = userCallPriceDal;
        _walletDal = walletDal;
        _walletLogDAL = walletLogDAL;
    }

    public async Task<IDataResult<CallRD>> Create(CallWD callWD)
    {
        try
        {
            callWD.CreationDate = DateTime.Now;
            callWD.Status = (int)ECallStatus.Started;
            callWD.IsActive = true;
            var data = await _callDal.AddAsync(_mapper.Map<Call>(callWD));
            return new SuccessDataResult<CallRD>(_mapper.Map<CallRD>(data), Messages.AddingProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Create), new { data = callWD });
            return new ErrorDataResult<CallRD>(Messages.AddingProcessFailed);
        }
    }

    public async Task<bool> Delete(long id, long userId)
    {
        try
        {
            var call = await _callDal.GetAsync(x => x.Id == id && x.CallerUserId == userId);
            if (call != null)
            {
                call.IsActive = false;
                call.UpdateDate = DateTime.Now;
                await _callDal.UpdateAsync(call);
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Delete), new { data = id });
            throw;
        }
    }

    public async Task<IDataResult<List<CallRD>>> Get(Expression<Func<Entities.Models.Call, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10)
    {
        try
        {
            var calls = await _callDal.GetPageAsync(filter, includes, (int)pageNumber!, (int)pageSize!);
            return new SuccessDataResult<List<CallRD>>(_mapper.Map<List<CallRD>>(calls), Messages.ProcessSuccess, await _callDal.CountAsync(filter));
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Get), new { data = filter });
            throw new Exception(Messages.GetProcessFailed, ex);
        }
    }

    public async Task<CallRD?> GetByExpression(Expression<Func<Entities.Models.Call, bool>> filter = null, string[] includes = null)
    {
        try
        {
            var result = await _callDal.GetAsync(filter, includes);
            if (result != null)
                return _mapper.Map<CallRD>(result);

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(GetByExpression), new { data = filter });
            throw new Exception(Messages.ProcessFailed, ex);
        }
    }

    public async Task<IDataResult<CallRD>> Update(CallWD callWD)
    {
        try
        {
            callWD.UpdateDate = DateTime.Now;
            if (callWD.Status == (int)ECallStatus.Completed)
            {
                callWD.EndTime = DateTime.Now;
                callWD.TotalDurationInSeconds = (int)(callWD.EndTime - callWD.StartTime)?.TotalSeconds;

                var userCallPrice = await _userCallPriceDal.GetAsync(x => x.UserId == callWD.ReceiverUserId && x.IsActive);
                if (userCallPrice != null)
                {
                    //TODO: bakiye kontrol gibi iþlemler eklenmeli
                    var min = callWD.TotalDurationInSeconds / 60;
                    callWD.TotalPrice = min * userCallPrice.RatePerMinuute;

                    var userWallet = await _walletDal.GetAsync(x => x.UserId == callWD.CallerUserId && x.IsActive);
                    userWallet.Balance -= callWD.TotalPrice ?? 0;
                    await _walletDal.UpdateAsync(userWallet);

                    await _walletLogDAL.AddAsync(new WalletLog
                    {
                        Amount = -(callWD.TotalPrice ?? 0),
                        CreationDate = DateTime.Now,
                        IsActive = true,
                        WalletId = userWallet.Id,
                        UpdateDate = DateTime.Now,
                    });

                    await _callPaymentDal.AddAsync(new CallPayment
                    {
                        CallId = callWD.Id,
                        PayerId = callWD.CallerUserId,
                        Amount = callWD.TotalPrice ?? 0,
                        PaidDate = DateTime.Now,
                        PaymentStatus = (int)EPaymentStatus.Completed,
                    });
                }
                else
                {
                    callWD.TotalPrice = 0;
                }
            }
            else
            {
                callWD.TotalDurationInSeconds = 0;
                callWD.TotalPrice = 0;
            }

            await _callDal.UpdateAsync(_mapper.Map<Call>(callWD));
            return new SuccessDataResult<CallRD>(_mapper.Map<CallRD>(callWD), Messages.UpdateProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Update), new { data = callWD });
            return new ErrorDataResult<CallRD>(Messages.UpdateProcessFailed);
        }
    }

    public async Task<long> Count(Expression<Func<Call, bool>>? filter = null, string[]? includes = null)
    {
        try
        {
            return await _callDal.CountAsync(filter, includes);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Count), new { data = "" });
            throw;
        }
    }
}