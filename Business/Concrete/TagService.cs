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
public class TagService : ITagService
{
    private readonly IMapper _mapper;
    readonly ILogger _logger;
    ITagDAL _tagDal;

    public TagService(ITagDAL tagDal, IMapper mapper, ILogger logger)
    {
        _tagDal = tagDal;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IDataResult<TagRD>> Create(TagWD tagWD)
    {
        try
        {
            var dataCheck = await _tagDal.GetAsync(x => x.Name == tagWD.Name && x.IsActive == true);
            if (dataCheck == null)
            {
                var data = await _tagDal.AddAsync(_mapper.Map<Tag>(tagWD));
                return new SuccessDataResult<TagRD>(_mapper.Map<TagRD>(data), Messages.AddingProcessSuccessful);
            }

            return new SuccessDataResult<TagRD>(_mapper.Map<TagRD>(tagWD), Messages.AddingProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Create), new { data = tagWD });
            return new ErrorDataResult<TagRD>(Messages.AddingProcessFailed);
        }
    }

    public async Task<bool> Delete(long id)
    {
        try
        {
            Tag tag = await _tagDal.GetAsync(x => x.Id == id);
            if (tag != null)
            {
                tag.IsActive = false;
                await _tagDal.UpdateAsync(tag);
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Delete), new { data = id });
            throw;
        }
    }

    public async Task<IDataResult<List<TagRD>>> Get(Expression<Func<Entities.Models.Tag, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10)
    {
        try
        {
            var tags = await _tagDal.GetPageAsync(filter, includes, (int)pageNumber!, (int)pageSize!);
            return new SuccessDataResult<List<TagRD>>(_mapper.Map<List<TagRD>>(tags), Messages.ProcessSuccess, await _tagDal.CountAsync(filter));
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Get), new { data = filter });
            throw new Exception(Messages.GetProcessFailed, ex);
        }
    }

    public async Task<TagRD?> GetByExpression(Expression<Func<Entities.Models.Tag, bool>> filter = null, string[] includes = null)
    {
        try
        {
            var result = await _tagDal.GetAsync(filter, includes);
            if (result != null)
                return _mapper.Map<TagRD>(result);

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(GetByExpression), new { data = filter });
            throw new Exception(Messages.ProcessFailed, ex);
        }
    }

    public async Task<IDataResult<TagRD>> Update(TagWD tagWD)
    {
        try
        {
            var dataCheck = await _tagDal.GetAsync(x => x.Name == tagWD.Name && x.IsActive == true && x.Id != tagWD.Id);
            if (dataCheck == null)
            {
                await _tagDal.UpdateAsync(_mapper.Map<Tag>(tagWD));
                return new SuccessDataResult<TagRD>(_mapper.Map<TagRD>(tagWD), Messages.UpdateProcessSuccessful);
            }

            return new ErrorDataResult<TagRD>(Messages.TagAlreadyExists);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Update), new { data = tagWD });
            return new ErrorDataResult<TagRD>(Messages.UpdateProcessFailed);
        }
    }

    public async Task<long> Count(Expression<Func<Tag, bool>>? filter = null, string[]? includes = null)
    {
        try
        {
            return await _tagDal.CountAsync(filter, includes);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Count), new { data = "" });
            throw;
        }
    }
}