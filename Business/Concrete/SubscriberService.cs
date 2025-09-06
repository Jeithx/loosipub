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
public class SubscriberService : ISubscriberService
{
    private readonly IMapper _mapper;
    readonly ILogger _logger;
    ISubscriberDAL _subscriberDal;

    public SubscriberService(ISubscriberDAL subscriberDal, IMapper mapper, ILogger logger)
    {
        _subscriberDal = subscriberDal;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IDataResult<SubscriberRD>> Create(SubscriberWD subscriberWD)
    {
        try
        {
            var data = await _subscriberDal.AddAsync(_mapper.Map<Subscriber>(subscriberWD));
            return new SuccessDataResult<SubscriberRD>(_mapper.Map<SubscriberRD>(data), Messages.AddingProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Create), new { data = subscriberWD });
            return new ErrorDataResult<SubscriberRD>(Messages.AddingProcessFailed);
        }
    }

    public async Task<bool> Delete(long id, long userId)
    {
        try
        {
            Subscriber subscriber = await _subscriberDal.GetAsync(x => x.Id == id && x.UserId == userId && x.IsActive);
            if (subscriber != null)
            {
                await _subscriberDal.DeleteAsync(subscriber);
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Delete), new { data = id });
            throw;
        }
    }

    public async Task<IDataResult<List<SubscriberRD>>> Get(Expression<Func<Entities.Models.Subscriber, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10)
    {
        try
        {
            var subscribers = await _subscriberDal.GetPageAsync(filter, includes, (int)pageNumber!, (int)pageSize!);
            return new SuccessDataResult<List<SubscriberRD>>(_mapper.Map<List<SubscriberRD>>(subscribers), Messages.ProcessSuccess, await _subscriberDal.CountAsync(filter));
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Get), new { data = filter });
            throw new Exception(Messages.GetProcessFailed, ex);
        }
    }

    public async Task<SubscriberRD?> GetByExpression(Expression<Func<Entities.Models.Subscriber, bool>> filter = null, string[] includes = null)
    {
        try
        {
            var result = await _subscriberDal.GetAsync(filter, includes);
            if (result != null)
                return _mapper.Map<SubscriberRD>(result);

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(GetByExpression), new { data = filter });
            throw new Exception(Messages.ProcessFailed, ex);
        }
    }

    public async Task<IDataResult<SubscriberRD>> Update(SubscriberWD subscriberWD)
    {
        try
        {
            await _subscriberDal.UpdateAsync(_mapper.Map<Subscriber>(subscriberWD));
            return new SuccessDataResult<SubscriberRD>(_mapper.Map<SubscriberRD>(subscriberWD), Messages.UpdateProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Update), new { data = subscriberWD });
            return new ErrorDataResult<SubscriberRD>(Messages.UpdateProcessFailed);
        }
    }

    public async Task<long> Count(Expression<Func<Subscriber, bool>>? filter = null, string[]? includes = null)
    {
        try
        {
            return await _subscriberDal.CountAsync(filter, includes);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Count), new { data = "" });
            throw;
        }
    }
}