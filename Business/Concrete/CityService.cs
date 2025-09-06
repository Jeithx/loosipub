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
public class CityService : ICityService
{
    private readonly IMapper _mapper;
    readonly ILogger _logger;
    ICityDAL _cityDal;

    public CityService(ICityDAL cityDal, IMapper mapper, ILogger logger)
    {
        _cityDal = cityDal;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IDataResult<CityRD>> Create(CityWD cityWD)
    {
        try
        {
            var data = await _cityDal.AddAsync(_mapper.Map<City>(cityWD));
            return new SuccessDataResult<CityRD>(_mapper.Map<CityRD>(data), Messages.AddingProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Create), new { data = cityWD });
            return new ErrorDataResult<CityRD>(Messages.AddingProcessFailed);
        }
    }

    public async Task<bool> Delete(long id)
    {
        try
        {
            var city = await _cityDal.GetAsync(x => x.Id == id);
            if (city != null)
            {
                city.IsActive = false;
                await _cityDal.UpdateAsync(city);
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Delete), new { data = id });
            throw;
        }
    }

    public async Task<IDataResult<List<CityRD>>> Get(Expression<Func<Entities.Models.City, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10, bool notPagination = false)
    {
        try
        {
            List<City> citys;

            if (notPagination==false)
            {
                 citys = await _cityDal.GetPageAsync(filter, null, x => x.Name, includes, (int)pageNumber!, (int)pageSize!);
            }
            else
            {
                 citys = await _cityDal.GetListAsync(filter,includes);
            }
            return new SuccessDataResult<List<CityRD>>(_mapper.Map<List<CityRD>>(citys), Messages.ProcessSuccess, await _cityDal.CountAsync(filter));
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Get), new { data = filter });
            throw new Exception(Messages.GetProcessFailed, ex);
        }
    }

    public async Task<CityRD?> GetByExpression(Expression<Func<Entities.Models.City, bool>> filter = null, string[] includes = null)
    {
        try
        {
            var result = await _cityDal.GetAsync(filter, includes);
            if (result != null)
                return _mapper.Map<CityRD>(result);

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(GetByExpression), new { data = filter });
            throw new Exception(Messages.ProcessFailed, ex);
        }
    }

    public async Task<IDataResult<CityRD>> Update(CityWD cityWD)
    {
        try
        {
            await _cityDal.UpdateAsync(_mapper.Map<City>(cityWD));
            return new SuccessDataResult<CityRD>(_mapper.Map<CityRD>(cityWD), Messages.UpdateProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Update), new { data = cityWD });
            return new ErrorDataResult<CityRD>(Messages.UpdateProcessFailed);
        }
    }

    public async Task<long> Count(Expression<Func<City, bool>>? filter = null, string[]? includes = null)
    {
        try
        {
            return await _cityDal.CountAsync(filter, includes);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Count), new { data = "" });
            throw;
        }
    }
}