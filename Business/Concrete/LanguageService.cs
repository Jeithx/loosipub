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
public class LanguageService : ILanguageService
{
    private readonly IMapper _mapper;
    readonly ILogger _logger;
    ILanguageDAL _languageDal;

    public LanguageService(ILanguageDAL languageDal, IMapper mapper, ILogger logger)
    {
        _languageDal = languageDal;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IDataResult<LanguageRD>> Create(LanguageWD languageWD)
    {
        try
        {
            var data = await _languageDal.AddAsync(_mapper.Map<Language>(languageWD));
            return new SuccessDataResult<LanguageRD>(_mapper.Map<LanguageRD>(data), Messages.AddingProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Create), new { data = languageWD });
            return new ErrorDataResult<LanguageRD>(Messages.AddingProcessFailed);
        }
    }

    public async Task<bool> Delete(long id)
    {
        try
        {
            Language language = await _languageDal.GetAsync(x => x.Id == id);

            await _languageDal.UpdateAsync(language);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Delete), new { data = id });
            throw;
        }
    }

    public async Task<IDataResult<List<LanguageRD>>> Get(Expression<Func<Entities.Models.Language, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10)
    {
        try
        {
            var languages = await _languageDal.GetPageAsync(filter, includes, (int)pageNumber!, (int)pageSize!);
            return new SuccessDataResult<List<LanguageRD>>(_mapper.Map<List<LanguageRD>>(languages), Messages.ProcessSuccess, await _languageDal.CountAsync(filter));
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Get), new { data = filter });
            throw new Exception(Messages.GetProcessFailed, ex);
        }
    }

    public async Task<LanguageRD?> GetByExpression(Expression<Func<Entities.Models.Language, bool>> filter = null, string[] includes = null)
    {
        try
        {
            var result = await _languageDal.GetAsync(filter, includes);
            if (result != null)
                return _mapper.Map<LanguageRD>(result);

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(GetByExpression), new { data = filter });
            throw new Exception(Messages.ProcessFailed, ex);
        }
    }

    public async Task<IDataResult<LanguageRD>> Update(LanguageWD languageWD)
    {
        try
        {
            await _languageDal.UpdateAsync(_mapper.Map<Language>(languageWD));
            return new SuccessDataResult<LanguageRD>(_mapper.Map<LanguageRD>(languageWD), Messages.UpdateProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Update), new { data = languageWD });
            return new ErrorDataResult<LanguageRD>(Messages.UpdateProcessFailed);
        }
    }

    public async Task<long> Count(Expression<Func<Language, bool>>? filter = null, string[]? includes = null)
    {
        try
        {
            return await _languageDal.CountAsync(filter, includes);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Count), new { data = "" });
            throw;
        }
    }
}