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
public class CountryService : ICountryService
{
    private readonly IMapper _mapper;
    readonly ILogger _logger;
    ICountryDAL _countryDal;

    public CountryService(ICountryDAL countryDal, IMapper mapper, ILogger logger)
    {
        _countryDal = countryDal;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IDataResult<CountryRD>> Create(CountryWD countryWD)
    {
        try
        {
            var data = await _countryDal.AddAsync(_mapper.Map<Country>(countryWD));
            return new SuccessDataResult<CountryRD>(_mapper.Map<CountryRD>(data), Messages.AddingProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Create), new { data = countryWD });
            return new ErrorDataResult<CountryRD>(Messages.AddingProcessFailed);
        }
    }

    public async Task<bool> Delete(long id)
    {
        try
        {
            Country country = await _countryDal.GetAsync(x => x.Id == id);

            await _countryDal.UpdateAsync(country);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Delete), new { data = id });
            throw;
        }
    }

    public async Task<IDataResult<List<CountryRD>>> Get(Expression<Func<Entities.Models.Country, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10,bool notPagination=false)
    {
        try
        {
            List<Country> countrys;
            if (notPagination==false)
            {
                 countrys = await _countryDal.GetPageAsync(filter, null, x => x.Name, includes, (int)pageNumber!, (int)pageSize!);
            }
            else
            {
                 countrys = await _countryDal.GetListAsync(filter,includes);
            }

            return new SuccessDataResult<List<CountryRD>>(_mapper.Map<List<CountryRD>>(countrys), Messages.ProcessSuccess, await _countryDal.CountAsync(filter));
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Get), new { data = filter });
            throw new Exception(Messages.GetProcessFailed, ex);
        }
    }

    public async Task<CountryRD> GetByExpression(Expression<Func<Entities.Models.Country, bool>> filter = null, string[] includes = null)
    {
        try
        {
            var result = await _countryDal.GetAsync(filter, includes);
            if (result != null)
                return _mapper.Map<CountryRD>(result);

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(GetByExpression), new { data = filter });
            throw new Exception(Messages.ProcessFailed, ex);
        }
    }

    public async Task<IDataResult<CountryRD>> Update(CountryWD countryWD)
    {
        try
        {
            await _countryDal.UpdateAsync(_mapper.Map<Country>(countryWD));
            return new SuccessDataResult<CountryRD>(_mapper.Map<CountryRD>(countryWD), Messages.UpdateProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Update), new { data = countryWD });
            return new ErrorDataResult<CountryRD>(Messages.UpdateProcessFailed);
        }
    }

    public async Task<long> Count(Expression<Func<Country, bool>>? filter = null, string[]? includes = null)
    {
        try
        {
            return await _countryDal.CountAsync(filter, includes);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Count), new { data = "" });
            throw;
        }
    }
}