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
public class GenderService : IGenderService
{
    private readonly IMapper _mapper;
    readonly ILogger _logger;
    IGenderDAL _genderDal;

    public GenderService(IGenderDAL genderDal, IMapper mapper, ILogger logger)
    {
        _genderDal = genderDal;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IDataResult<GenderRD>> Create(GenderWD genderWD)
    {
        try
        {
            var data = await _genderDal.AddAsync(_mapper.Map<Gender>(genderWD));
            return new SuccessDataResult<GenderRD>(_mapper.Map<GenderRD>(data), Messages.AddingProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Create), new { data = genderWD });
            return new ErrorDataResult<GenderRD>(Messages.AddingProcessFailed);
        }
    }

    public async Task<bool> Delete(long id)
    {
        try
        {
            Gender gender = await _genderDal.GetAsync(x => x.Id == id);
            if (gender != null)
            {
                gender.IsActive = false;
                await _genderDal.UpdateAsync(gender);
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Delete), new { data = id });
            throw;
        }
    }

    public async Task<IDataResult<List<GenderRD>>> Get(Expression<Func<Entities.Models.Gender, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10)
    {
        try
        {
            var genders = await _genderDal.GetPageAsync(filter, includes, (int)pageNumber!, (int)pageSize!);
            return new SuccessDataResult<List<GenderRD>>(_mapper.Map<List<GenderRD>>(genders), Messages.ProcessSuccess, await _genderDal.CountAsync(filter));
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Get), new { data = filter });
            throw new Exception(Messages.GetProcessFailed, ex);
        }
    }

    public async Task<GenderRD?> GetByExpression(Expression<Func<Entities.Models.Gender, bool>> filter = null, string[] includes = null)
    {
        try
        {
            var result = await _genderDal.GetAsync(filter, includes);
            if (result != null)
                return _mapper.Map<GenderRD>(result);

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(GetByExpression), new { data = filter });
            throw new Exception(Messages.ProcessFailed, ex);
        }
    }

    public async Task<IDataResult<GenderRD>> Update(GenderWD genderWD)
    {
        try
        {
            await _genderDal.UpdateAsync(_mapper.Map<Gender>(genderWD));
            return new SuccessDataResult<GenderRD>(_mapper.Map<GenderRD>(genderWD), Messages.UpdateProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Update), new { data = genderWD });
            return new ErrorDataResult<GenderRD>(Messages.UpdateProcessFailed);
        }
    }

    public async Task<long> Count(Expression<Func<Gender, bool>>? filter = null, string[]? includes = null)
    {
        try
        {
            return await _genderDal.CountAsync(filter, includes);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Count), new { data = "" });
            throw;
        }
    }
}