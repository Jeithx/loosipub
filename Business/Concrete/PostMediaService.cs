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
public class PostMediaService : IPostMediaService
{
    private readonly IMapper _mapper;
    readonly ILogger _logger;
    IPostMediaDAL _postmediaDal;

    public PostMediaService(IPostMediaDAL postmediaDal, IMapper mapper, ILogger logger)
    {
        _postmediaDal = postmediaDal;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IDataResult<PostMediaRD>> Create(PostMediaWD postmediaWD)
    {
        try
        {
            var data = await _postmediaDal.AddAsync(_mapper.Map<PostMedia>(postmediaWD));
            return new SuccessDataResult<PostMediaRD>(_mapper.Map<PostMediaRD>(data), Messages.AddingProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Create), new { data = postmediaWD });
            return new ErrorDataResult<PostMediaRD>(Messages.AddingProcessFailed);
        }
    }

    public async Task<bool> Delete(long id)
    {
        try
        {
            PostMedia postmedia = await _postmediaDal.GetAsync(x => x.Id == id);
            if (postmedia != null)
            {
                postmedia.IsActive = false;
                await _postmediaDal.UpdateAsync(postmedia);
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Delete), new { data = id });
            throw;
        }
    }

    public async Task<IDataResult<List<PostMediaRD>>> Get(Expression<Func<Entities.Models.PostMedia, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10)
    {
        try
        {
            var postmedias = await _postmediaDal.GetPageAsync(filter, null, x => x.Order, includes, (int)pageNumber!, (int)pageSize!);
            return new SuccessDataResult<List<PostMediaRD>>(_mapper.Map<List<PostMediaRD>>(postmedias), Messages.ProcessSuccess, await _postmediaDal.CountAsync(filter));
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Get), new { data = filter });
            throw new Exception(Messages.GetProcessFailed, ex);
        }
    }

    public async Task<PostMediaRD?> GetByExpression(Expression<Func<Entities.Models.PostMedia, bool>> filter = null, string[] includes = null)
    {
        try
        {
            var result = await _postmediaDal.GetAsync(filter, includes);
            if (result != null)
                return _mapper.Map<PostMediaRD>(result);

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(GetByExpression), new { data = filter });
            throw new Exception(Messages.ProcessFailed, ex);
        }
    }

    public async Task<IDataResult<PostMediaRD>> Update(PostMediaWD postmediaWD)
    {
        try
        {
            await _postmediaDal.UpdateAsync(_mapper.Map<PostMedia>(postmediaWD));
            return new SuccessDataResult<PostMediaRD>(_mapper.Map<PostMediaRD>(postmediaWD), Messages.UpdateProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Update), new { data = postmediaWD });
            return new ErrorDataResult<PostMediaRD>(Messages.UpdateProcessFailed);
        }
    }

    public async Task<long> Count(Expression<Func<PostMedia, bool>>? filter = null, string[]? includes = null)
    {
        try
        {
            return await _postmediaDal.CountAsync(filter, includes);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Count), new { data = "" });
            throw;
        }
    }
}