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
public class UserLoginAttemptService : IUserLoginAttemptService
{
    private readonly IMapper _mapper;
    readonly ILogger _logger;
    IUserLoginAttemptDAL _userloginattemptDal;

    public UserLoginAttemptService(IUserLoginAttemptDAL userloginattemptDal, IMapper mapper, ILogger logger)
    {
        _userloginattemptDal = userloginattemptDal;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IDataResult<UserLoginAttemptRD>> Create(UserLoginAttemptWD userloginattemptWD)
    {
        try
        {
            userloginattemptWD.AttemptDate = DateTime.Now;
            var data = await _userloginattemptDal.AddAsync(_mapper.Map<UserLoginAttempt>(userloginattemptWD));
            return new SuccessDataResult<UserLoginAttemptRD>(_mapper.Map<UserLoginAttemptRD>(data), Messages.AddingProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Create), new { data = userloginattemptWD });
            return new ErrorDataResult<UserLoginAttemptRD>(Messages.AddingProcessFailed);
        }
    }

    public async Task<IDataResult<List<UserLoginAttemptRD>>> Get(Expression<Func<Entities.Models.UserLoginAttempt, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10)
    {
        try
        {
            var userloginattempts = await _userloginattemptDal.GetPageAsync(filter, includes, (int)pageNumber!, (int)pageSize!);
            return new SuccessDataResult<List<UserLoginAttemptRD>>(_mapper.Map<List<UserLoginAttemptRD>>(userloginattempts), Messages.ProcessSuccess, await _userloginattemptDal.CountAsync(filter));
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Get), new { data = filter });
            throw new Exception(Messages.GetProcessFailed, ex);
        }
    }

    public async Task<UserLoginAttemptRD?> GetByExpression(Expression<Func<Entities.Models.UserLoginAttempt, bool>> filter = null, string[] includes = null)
    {
        try
        {
            var result = await _userloginattemptDal.GetAsync(filter, includes);
            if (result != null)
                return _mapper.Map<UserLoginAttemptRD>(result);

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(GetByExpression), new { data = filter });
            throw new Exception(Messages.ProcessFailed, ex);
        }
    }

    public async Task<IDataResult<UserLoginAttemptRD>> Update(UserLoginAttemptWD userloginattemptWD)
    {
        try
        {
            await _userloginattemptDal.UpdateAsync(_mapper.Map<UserLoginAttempt>(userloginattemptWD));
            return new SuccessDataResult<UserLoginAttemptRD>(_mapper.Map<UserLoginAttemptRD>(userloginattemptWD), Messages.UpdateProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Update), new { data = userloginattemptWD });
            return new ErrorDataResult<UserLoginAttemptRD>(Messages.UpdateProcessFailed);
        }
    }

    public async Task<long> Count(Expression<Func<UserLoginAttempt, bool>>? filter = null, string[]? includes = null)
    {
        try
        {
            return await _userloginattemptDal.CountAsync(filter, includes);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Count), new { data = "" });
            throw;
        }
    }
}