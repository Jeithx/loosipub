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
public class CallPaymentService : ICallPaymentService
{
    private readonly IMapper _mapper;
    readonly ILogger _logger;
    ICallPaymentDAL _callpaymentDal;

    public CallPaymentService(ICallPaymentDAL callpaymentDal, IMapper mapper, ILogger logger)
    {
        _callpaymentDal = callpaymentDal;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IDataResult<CallPaymentRD>> Create(CallPaymentWD callpaymentWD)
    {
        try
        {
            callpaymentWD.PaidDate = DateTime.Now;
            var data = await _callpaymentDal.AddAsync(_mapper.Map<CallPayment>(callpaymentWD));
            return new SuccessDataResult<CallPaymentRD>(_mapper.Map<CallPaymentRD>(data), Messages.AddingProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Create), new { data = callpaymentWD });
            return new ErrorDataResult<CallPaymentRD>(Messages.AddingProcessFailed);
        }
    }

    public async Task<bool> Delete(long id, long userId)
    {
        try
        {
            var callpayment = await _callpaymentDal.GetAsync(x => x.Id == id && x.PayerId == userId);
            if (callpayment != null)
            {
                await _callpaymentDal.DeleteAsync(callpayment);
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Delete), new { data = id });
            throw;
        }
    }

    public async Task<IDataResult<List<CallPaymentRD>>> Get(Expression<Func<Entities.Models.CallPayment, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10)
    {
        try
        {
            var callpayments = await _callpaymentDal.GetPageAsync(filter, includes, (int)pageNumber!, (int)pageSize!);
            return new SuccessDataResult<List<CallPaymentRD>>(_mapper.Map<List<CallPaymentRD>>(callpayments), Messages.ProcessSuccess, await _callpaymentDal.CountAsync(filter));
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Get), new { data = filter });
            throw new Exception(Messages.GetProcessFailed, ex);
        }
    }

    public async Task<CallPaymentRD?> GetByExpression(Expression<Func<Entities.Models.CallPayment, bool>> filter = null, string[] includes = null)
    {
        try
        {
            var result = await _callpaymentDal.GetAsync(filter, includes);
            if (result != null)
                return _mapper.Map<CallPaymentRD>(result);

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(GetByExpression), new { data = filter });
            throw new Exception(Messages.ProcessFailed, ex);
        }
    }

    public async Task<IDataResult<CallPaymentRD>> Update(CallPaymentWD callpaymentWD)
    {
        try
        {
            await _callpaymentDal.UpdateAsync(_mapper.Map<CallPayment>(callpaymentWD));
            return new SuccessDataResult<CallPaymentRD>(_mapper.Map<CallPaymentRD>(callpaymentWD), Messages.UpdateProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Update), new { data = callpaymentWD });
            return new ErrorDataResult<CallPaymentRD>(Messages.UpdateProcessFailed);
        }
    }

    public async Task<long> Count(Expression<Func<CallPayment, bool>>? filter = null, string[]? includes = null)
    {
        try
        {
            return await _callpaymentDal.CountAsync(filter, includes);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Count), new { data = "" });
            throw;
        }
    }
}