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
public class UserCallPriceService : IUserCallPriceService
{
    private readonly IMapper _mapper;
    readonly ILogger _logger;
    IUserCallPriceDAL _usercallpriceDal;

    public UserCallPriceService(IUserCallPriceDAL usercallpriceDal, IMapper mapper, ILogger logger)
    {
        _usercallpriceDal = usercallpriceDal;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IDataResult<UserCallPriceRD>> Create(UserCallPriceWD usercallpriceWD)
    {
        try
        {
            usercallpriceWD.CreationDate = DateTime.UtcNow;
            var data = await _usercallpriceDal.AddAsync(_mapper.Map<UserCallPrice>(usercallpriceWD));
            return new SuccessDataResult<UserCallPriceRD>(_mapper.Map<UserCallPriceRD>(data), Messages.AddingProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Create), new { data = usercallpriceWD });
            return new ErrorDataResult<UserCallPriceRD>(Messages.AddingProcessFailed);
        }
    }

    public async Task<bool> Delete(long id, long userId)
    {
        try
        {
            UserCallPrice usercallprice = await _usercallpriceDal.GetAsync(x => x.Id == id && x.UserId == userId);
            if (usercallprice != null)
            {
                usercallprice.IsActive = false;
                usercallprice.UpdateDate = DateTime.Now;
                await _usercallpriceDal.UpdateAsync(usercallprice);
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Delete), new { data = id });
            throw;
        }
    }

    public async Task<IDataResult<List<UserCallPriceRD>>> Get(Expression<Func<Entities.Models.UserCallPrice, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10)
    {
        try
        {
            var usercallprices = await _usercallpriceDal.GetPageAsync(filter, includes, (int)pageNumber!, (int)pageSize!);
            return new SuccessDataResult<List<UserCallPriceRD>>(_mapper.Map<List<UserCallPriceRD>>(usercallprices), Messages.ProcessSuccess, await _usercallpriceDal.CountAsync(filter));
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Get), new { data = filter });
            throw new Exception(Messages.GetProcessFailed, ex);
        }
    }

    public async Task<UserCallPriceRD?> GetByExpression(Expression<Func<Entities.Models.UserCallPrice, bool>> filter = null, string[] includes = null)
    {
        try
        {
            var result = await _usercallpriceDal.GetAsync(filter, includes);
            if (result != null)
                return _mapper.Map<UserCallPriceRD>(result);

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(GetByExpression), new { data = filter });
            throw new Exception(Messages.ProcessFailed, ex);
        }
    }

    public async Task<IDataResult<UserCallPriceRD>> Update(UserCallPriceWD usercallpriceWD)
    {
        try
        {
            usercallpriceWD.UpdateDate = DateTime.Now;
            await _usercallpriceDal.UpdateAsync(_mapper.Map<UserCallPrice>(usercallpriceWD));
            return new SuccessDataResult<UserCallPriceRD>(_mapper.Map<UserCallPriceRD>(usercallpriceWD), Messages.UpdateProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Update), new { data = usercallpriceWD });
            return new ErrorDataResult<UserCallPriceRD>(Messages.UpdateProcessFailed);
        }
    }

    public async Task<long> Count(Expression<Func<UserCallPrice, bool>>? filter = null, string[]? includes = null)
    {
        try
        {
            return await _usercallpriceDal.CountAsync(filter, includes);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Count), new { data = "" });
            throw;
        }
    }
}