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
public class BlockedUserService : IBlockedUserService
{
    private readonly IMapper _mapper;
    readonly ILogger _logger;
    IBlockedUserDAL _blockeduserDal;

    public BlockedUserService(IBlockedUserDAL blockeduserDal, IMapper mapper, ILogger logger)
    {
        _blockeduserDal = blockeduserDal;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IDataResult<BlockedUserRD>> Create(BlockedUserWD blockeduserWD)
    {
        try
        {
            var checkData = await _blockeduserDal.GetAsync(x => x.UserId == blockeduserWD.UserId && x.BlockedUserId == blockeduserWD.BlockedUserId && x.IsActive);
            if (checkData == null)
            {
                var data = await _blockeduserDal.AddAsync(_mapper.Map<BlockedUser>(blockeduserWD));
                return new SuccessDataResult<BlockedUserRD>(_mapper.Map<BlockedUserRD>(data), Messages.AddingProcessSuccessful);
            }

            return new SuccessDataResult<BlockedUserRD>(_mapper.Map<BlockedUserRD>(blockeduserWD), Messages.AddingProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Create), new { data = blockeduserWD });
            return new ErrorDataResult<BlockedUserRD>(Messages.AddingProcessFailed);
        }
    }

    public async Task<bool> Delete(long id, long userId)
    {
        try
        {
            BlockedUser blockeduser = await _blockeduserDal.GetAsync(x => x.Id == id && x.UserId == userId && x.IsActive);
            if (blockeduser != null)
            {
                blockeduser.IsActive = false;
                await _blockeduserDal.UpdateAsync(blockeduser);
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Delete), new { data = id });
            throw;
        }
    }

    public async Task<IDataResult<List<BlockedUserRD>>> Get(Expression<Func<Entities.Models.BlockedUser, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10)
    {
        try
        {
            var blockedusers = await _blockeduserDal.GetPageAsync(filter, includes, (int)pageNumber!, (int)pageSize!);
            return new SuccessDataResult<List<BlockedUserRD>>(_mapper.Map<List<BlockedUserRD>>(blockedusers), Messages.ProcessSuccess, await _blockeduserDal.CountAsync(filter));
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Get), new { data = filter });
            throw new Exception(Messages.GetProcessFailed, ex);
        }
    }

    public async Task<BlockedUserRD?> GetByExpression(Expression<Func<Entities.Models.BlockedUser, bool>> filter = null, string[] includes = null)
    {
        try
        {
            var result = await _blockeduserDal.GetAsync(filter, includes);
            if (result != null)
                return _mapper.Map<BlockedUserRD>(result);

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(GetByExpression), new { data = filter });
            throw new Exception(Messages.ProcessFailed, ex);
        }
    }

    public async Task<IDataResult<BlockedUserRD>> Update(BlockedUserWD blockeduserWD)
    {
        try
        {
            await _blockeduserDal.UpdateAsync(_mapper.Map<BlockedUser>(blockeduserWD));
            return new SuccessDataResult<BlockedUserRD>(_mapper.Map<BlockedUserRD>(blockeduserWD), Messages.UpdateProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Update), new { data = blockeduserWD });
            return new ErrorDataResult<BlockedUserRD>(Messages.UpdateProcessFailed);
        }
    }

    public async Task<long> Count(Expression<Func<BlockedUser, bool>>? filter = null, string[]? includes = null)
    {
        try
        {
            return await _blockeduserDal.CountAsync(filter, includes);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Count), new { data = "" });
            throw;
        }
    }
}