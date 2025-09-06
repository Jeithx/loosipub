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
public class SocialMediaService : ISocialMediaService
{
    private readonly IMapper _mapper;
    readonly ILogger _logger;
    ISocialMediaDAL _socialmediaDal;

    public SocialMediaService(ISocialMediaDAL socialmediaDal, IMapper mapper, ILogger logger)
    {
        _socialmediaDal = socialmediaDal;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IDataResult<SocialMediaRD>> Create(SocialMediaWD socialmediaWD)
    {
        try
        {
            var data = await _socialmediaDal.AddAsync(_mapper.Map<SocialMedia>(socialmediaWD));
            return new SuccessDataResult<SocialMediaRD>(_mapper.Map<SocialMediaRD>(data), Messages.AddingProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Create), new { data = socialmediaWD });
            return new ErrorDataResult<SocialMediaRD>(Messages.AddingProcessFailed);
        }
    }

    public async Task<bool> Delete(long id)
    {
        try
        {
            SocialMedia socialmedia = await _socialmediaDal.GetAsync(x => x.Id == id);
            if (socialmedia != null)
            {
                socialmedia.IsActive = false;
                await _socialmediaDal.UpdateAsync(socialmedia);
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Delete), new { data = id });
            throw;
        }
    }

    public async Task<IDataResult<List<SocialMediaRD>>> Get(Expression<Func<Entities.Models.SocialMedia, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10)
    {
        try
        {
            var socialmedias = await _socialmediaDal.GetPageAsync(filter, includes, (int)pageNumber!, (int)pageSize!);
            return new SuccessDataResult<List<SocialMediaRD>>(_mapper.Map<List<SocialMediaRD>>(socialmedias), Messages.ProcessSuccess, await _socialmediaDal.CountAsync(filter));
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Get), new { data = filter });
            throw new Exception(Messages.GetProcessFailed, ex);
        }
    }

    public async Task<SocialMediaRD?> GetByExpression(Expression<Func<Entities.Models.SocialMedia, bool>> filter = null, string[] includes = null)
    {
        try
        {
            var result = await _socialmediaDal.GetAsync(filter, includes);
            if (result != null)
                return _mapper.Map<SocialMediaRD>(result);

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(GetByExpression), new { data = filter });
            throw new Exception(Messages.ProcessFailed, ex);
        }
    }

    public async Task<IDataResult<SocialMediaRD>> Update(SocialMediaWD socialmediaWD)
    {
        try
        {
            await _socialmediaDal.UpdateAsync(_mapper.Map<SocialMedia>(socialmediaWD));
            return new SuccessDataResult<SocialMediaRD>(_mapper.Map<SocialMediaRD>(socialmediaWD), Messages.UpdateProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Update), new { data = socialmediaWD });
            return new ErrorDataResult<SocialMediaRD>(Messages.UpdateProcessFailed);
        }
    }

    public async Task<long> Count(Expression<Func<SocialMedia, bool>>? filter = null, string[]? includes = null)
    {
        try
        {
            return await _socialmediaDal.CountAsync(filter, includes);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Count), new { data = "" });
            throw;
        }
    }
}