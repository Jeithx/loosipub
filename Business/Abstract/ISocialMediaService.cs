using Core.Models;
using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using System.Linq.Expressions;
using Entities.Models;

namespace Business.Abstract;

public interface ISocialMediaService
{
    Task<IDataResult<SocialMediaRD>> Create(SocialMediaWD model);
    Task<IDataResult<SocialMediaRD>> Update(SocialMediaWD model);
    Task<bool> Delete(long id);
    Task<IDataResult<List<SocialMediaRD>>> Get(Expression<Func<Entities.Models.SocialMedia, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10);
 Task<SocialMediaRD> GetByExpression(Expression<Func<Entities.Models.SocialMedia, bool>> filter = null, string[]? includes = null);
    Task<long> Count(Expression<Func<Entities.Models.SocialMedia, bool>>? filter = null, string[]? includes = null);
}