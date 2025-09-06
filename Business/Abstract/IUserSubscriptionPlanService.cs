using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using System.Linq.Expressions;

namespace Business.Abstract;

public interface IUserSubscriptionPlanService
{
    Task<IDataResult<UserSubscriptionPlanRD>> Create(UserSubscriptionPlanWD model);
    Task<IDataResult<UserSubscriptionPlanRD>> Update(UserSubscriptionPlanWD model);
    Task<bool> Delete(long id, long userId);
    Task<IDataResult<List<UserSubscriptionPlanRD>>> Get(Expression<Func<Entities.Models.UserSubscriptionPlan, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10);
    Task<UserSubscriptionPlanRD> GetByExpression(Expression<Func<Entities.Models.UserSubscriptionPlan, bool>> filter = null, string[]? includes = null);
    Task<long> Count(Expression<Func<Entities.Models.UserSubscriptionPlan, bool>>? filter = null, string[]? includes = null);
}