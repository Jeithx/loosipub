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
public class PostTagService : IPostTagService
{
    private readonly IMapper _mapper;
    readonly ILogger _logger;
    IPostTagDAL _posttagDal;

    public PostTagService(IPostTagDAL posttagDal, IMapper mapper, ILogger logger)
    {
        _posttagDal = posttagDal;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IDataResult<PostTagRD>> Create(PostTagWD posttagWD)
    {
        try
        {
            var checkData = await _posttagDal.GetAsync(x => x.PostId == posttagWD.PostId && x.TagId == posttagWD.TagId);
            if (checkData == null)
            {
                var data = await _posttagDal.AddAsync(_mapper.Map<PostTag>(posttagWD));
                return new SuccessDataResult<PostTagRD>(_mapper.Map<PostTagRD>(data), Messages.AddingProcessSuccessful);
            }

            return new SuccessDataResult<PostTagRD>(_mapper.Map<PostTagRD>(posttagWD), Messages.AddingProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Create), new { data = posttagWD });
            return new ErrorDataResult<PostTagRD>(Messages.AddingProcessFailed);
        }
    }

    public async Task<bool> Delete(long postId, long tagId)
    {
        try
        {
            var posttag = await _posttagDal.GetAsync(x => x.PostId == postId && x.TagId == tagId);
            if (posttag != null)
                await _posttagDal.DeleteAsync(posttag);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Delete), new { data = postId });
            throw;
        }
    }

    public async Task<IDataResult<List<PostTagRD>>> Get(Expression<Func<Entities.Models.PostTag, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10)
    {
        try
        {
            var posttags = await _posttagDal.GetPageAsync(filter, includes, (int)pageNumber!, (int)pageSize!);
            return new SuccessDataResult<List<PostTagRD>>(_mapper.Map<List<PostTagRD>>(posttags), Messages.ProcessSuccess, await _posttagDal.CountAsync(filter));
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Get), new { data = filter });
            throw new Exception(Messages.GetProcessFailed, ex);
        }
    }

    public async Task<PostTagRD?> GetByExpression(Expression<Func<Entities.Models.PostTag, bool>> filter = null, string[] includes = null)
    {
        try
        {
            var result = await _posttagDal.GetAsync(filter, includes);
            if (result != null)
                return _mapper.Map<PostTagRD>(result);

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(GetByExpression), new { data = filter });
            throw new Exception(Messages.ProcessFailed, ex);
        }
    }

    public async Task<IDataResult<PostTagRD>> Update(PostTagWD posttagWD)
    {
        try
        {
            var checkData = await _posttagDal.GetAsync(x => x.PostId == posttagWD.PostId && x.TagId == posttagWD.TagId && x.Id != posttagWD.Id);
            if (checkData == null)
                await _posttagDal.UpdateAsync(_mapper.Map<PostTag>(posttagWD));

            return new SuccessDataResult<PostTagRD>(_mapper.Map<PostTagRD>(posttagWD), Messages.UpdateProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Update), new { data = posttagWD });
            return new ErrorDataResult<PostTagRD>(Messages.UpdateProcessFailed);
        }
    }

    public async Task<long> Count(Expression<Func<PostTag, bool>>? filter = null, string[]? includes = null)
    {
        try
        {
            return await _posttagDal.CountAsync(filter, includes);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Count), new { data = "" });
            throw;
        }
    }
}