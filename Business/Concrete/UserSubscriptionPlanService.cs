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
public class UserSubscriptionPlanService : IUserSubscriptionPlanService
{
    private readonly IMapper _mapper;
    readonly ILogger _logger;
    IUserSubscriptionPlanDAL _usersubscriptionplanDal;

    public UserSubscriptionPlanService(IUserSubscriptionPlanDAL usersubscriptionplanDal, IMapper mapper, ILogger logger)
    {
        _usersubscriptionplanDal = usersubscriptionplanDal;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IDataResult<UserSubscriptionPlanRD>> Create(UserSubscriptionPlanWD usersubscriptionplanWD)
    {
        try
        {
            usersubscriptionplanWD.CreationDate = DateTime.Now;
            usersubscriptionplanWD.IsActive = true;
            var data = await _usersubscriptionplanDal.AddAsync(_mapper.Map<UserSubscriptionPlan>(usersubscriptionplanWD));
            return new SuccessDataResult<UserSubscriptionPlanRD>(_mapper.Map<UserSubscriptionPlanRD>(data), Messages.AddingProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Create), new { data = usersubscriptionplanWD });
            return new ErrorDataResult<UserSubscriptionPlanRD>(Messages.AddingProcessFailed);
        }
    }

    public async Task<bool> Delete(long id, long userId)
    {
        try
        {
            var usersubscriptionplan = await _usersubscriptionplanDal.GetAsync(x => x.Id == id && x.ContentCreatorUserId == userId);
            if (usersubscriptionplan != null)
            {
                usersubscriptionplan.IsActive = false;
                usersubscriptionplan.UpdateDate = DateTime.Now;
                await _usersubscriptionplanDal.UpdateAsync(usersubscriptionplan);
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Delete), new { data = id });
            throw;
        }
    }

    public async Task<IDataResult<List<UserSubscriptionPlanRD>>> Get(Expression<Func<Entities.Models.UserSubscriptionPlan, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10)
    {
        try
        {
            var usersubscriptionplans = await _usersubscriptionplanDal.GetPageAsync(filter, includes, (int)pageNumber!, (int)pageSize!);
            return new SuccessDataResult<List<UserSubscriptionPlanRD>>(_mapper.Map<List<UserSubscriptionPlanRD>>(usersubscriptionplans), Messages.ProcessSuccess, await _usersubscriptionplanDal.CountAsync(filter));
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Get), new { data = filter });
            throw new Exception(Messages.GetProcessFailed, ex);
        }
    }

    public async Task<UserSubscriptionPlanRD?> GetByExpression(Expression<Func<Entities.Models.UserSubscriptionPlan, bool>> filter = null, string[] includes = null)
    {
        try
        {
            var result = await _usersubscriptionplanDal.GetAsync(filter, includes);
            if (result != null)
                return _mapper.Map<UserSubscriptionPlanRD>(result);

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(GetByExpression), new { data = filter });
            throw new Exception(Messages.ProcessFailed, ex);
        }
    }

    public async Task<IDataResult<UserSubscriptionPlanRD>> Update(UserSubscriptionPlanWD usersubscriptionplanWD)
    {
        try
        {
            await _usersubscriptionplanDal.UpdateAsync(_mapper.Map<UserSubscriptionPlan>(usersubscriptionplanWD));
            return new SuccessDataResult<UserSubscriptionPlanRD>(_mapper.Map<UserSubscriptionPlanRD>(usersubscriptionplanWD), Messages.UpdateProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Update), new { data = usersubscriptionplanWD });
            return new ErrorDataResult<UserSubscriptionPlanRD>(Messages.UpdateProcessFailed);
        }
    }

    public async Task<long> Count(Expression<Func<UserSubscriptionPlan, bool>>? filter = null, string[]? includes = null)
    {
        try
        {
            return await _usersubscriptionplanDal.CountAsync(filter, includes);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Count), new { data = "" });
            throw;
        }
    }
}