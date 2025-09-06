using AutoMapper;
using Business.Abstract;
using Core.Constants;
using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using CoreXNugetPackage.DataAccess.Dapper;
using Dapper;
using DataAccess.Abstract;
using Entities.Dtos;
using Entities.Models;
using System.Linq.Expressions;
namespace Business.Concrete;
public class MessageService : IMessageService
{
    private readonly IMapper _mapper;
    readonly ILogger _logger;
    IMessageDAL _messageDal;
    readonly DapperConnectionHelper _dapperConnectionHelper;
    readonly IUserService _userService;

    public MessageService(IMessageDAL messageDal, IMapper mapper, ILogger logger, DapperConnectionHelper dapperConnectionHelper, IUserService userService)
    {
        _messageDal = messageDal;
        _mapper = mapper;
        _logger = logger;
        _dapperConnectionHelper = dapperConnectionHelper;
        _userService = userService;
    }

    public async Task<IDataResult<MessageRD>> Create(MessageWD messageWD)
    {
        try
        {
            var user = await _userService.GetByExpression(x => x.Id == messageWD.ReceiverId && x.IsActive);
            if (user == null)
            {
                return new ErrorDataResult<MessageRD>(Messages.UserNotFound);
            }

            if (messageWD.SenderId == messageWD.ReceiverId)
            {
                return new ErrorDataResult<MessageRD>(Messages.SenderAndReceiverCannotBeSame);
            }

            var data = await _messageDal.AddAsync(_mapper.Map<Message>(messageWD));
            return new SuccessDataResult<MessageRD>(_mapper.Map<MessageRD>(data), Messages.AddingProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Create), new { data = messageWD });
            return new ErrorDataResult<MessageRD>(Messages.AddingProcessFailed);
        }
    }

    public async Task<bool> Delete(long id)
    {
        try
        {
            Message message = await _messageDal.GetAsync(x => x.Id == id);

            await _messageDal.UpdateAsync(message);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Delete), new { data = id });
            throw;
        }
    }

    public async Task<IDataResult<List<MessageRD>>> Get(Expression<Func<Entities.Models.Message, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10)
    {
        try
        {
            var messages = await _messageDal.GetPageAsync(filter, x => x.SendDate, null, includes, (int)pageNumber!, (int)pageSize!);
            return new SuccessDataResult<List<MessageRD>>(_mapper.Map<List<MessageRD>>(messages), Messages.ProcessSuccess, await _messageDal.CountAsync(filter));
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Get), new { data = filter });
            throw new Exception(Messages.GetProcessFailed, ex);
        }
    }

    public async Task<IDataResult<List<DapperWithGetMessage>>> GetGroupedMessages(long userId, int? pageNumber = 1, int? pageSize = 10)
    {
        try
        {
            var query =
       $@"
            WITH ConversationPartners AS (
                SELECT 
                    CASE 
                        WHEN m.SenderId = {userId} THEN m.ReceiverId
                        ELSE m.SenderId
                    END AS PartnerId,
                    m.SenderId,
                    m.ReceiverId,
                    m.SendDate,
                    m.Message
                FROM Messages m
                WHERE (m.SenderId = {userId} OR m.ReceiverId = {userId})
                    AND m.SenderId IS NOT NULL 
                    AND m.ReceiverId IS NOT NULL
            )
            SELECT 
                cp.PartnerId AS SenderId,
                {userId} AS ReceiverId,
                COUNT(*) AS MessageCount,
                MIN(cp.SendDate) AS FirstMessage,
                MAX(cp.SendDate) AS LastMessage,
                u.DisplayName,
                u.ProfilePictureUrl,
                (SELECT TOP 1 Message 
                 FROM Messages m2 
                 WHERE ((m2.SenderId = {userId} AND m2.ReceiverId = cp.PartnerId) 
                        OR (m2.SenderId = cp.PartnerId AND m2.ReceiverId = {userId}))
                 ORDER BY m2.SendDate DESC) AS LastMessageContent
            FROM ConversationPartners cp
            JOIN Users u ON u.Id = cp.PartnerId
            GROUP BY cp.PartnerId, u.DisplayName, u.ProfilePictureUrl
            ORDER BY LastMessage DESC 
            OFFSET ({pageNumber} - 1) * {pageSize} ROWS
            FETCH NEXT {pageSize} ROWS ONLY;";

            using var connection = _dapperConnectionHelper.CreateSqlConnection();
            var datas = connection.Query<DapperWithGetMessage>(query).ToList();

            return new SuccessDataResult<List<DapperWithGetMessage>>(datas, Messages.ProcessSuccess);

        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Get), new { data = userId });
            throw new Exception(Messages.GetProcessFailed, ex);
        }
    }

    public async Task<MessageRD?> GetByExpression(Expression<Func<Entities.Models.Message, bool>> filter = null, string[] includes = null)
    {
        try
        {
            var result = await _messageDal.GetAsync(filter, includes);
            if (result != null)
                return _mapper.Map<MessageRD>(result);

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(GetByExpression), new { data = filter });
            throw new Exception(Messages.ProcessFailed, ex);
        }
    }

    public async Task<IDataResult<MessageRD>> Update(MessageWD messageWD)
    {
        try
        {
            await _messageDal.UpdateAsync(_mapper.Map<Message>(messageWD));
            return new SuccessDataResult<MessageRD>(_mapper.Map<MessageRD>(messageWD), Messages.UpdateProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Update), new { data = messageWD });
            return new ErrorDataResult<MessageRD>(Messages.UpdateProcessFailed);
        }
    }

    public async Task<long> Count(Expression<Func<Message, bool>>? filter = null, string[]? includes = null)
    {
        try
        {
            return await _messageDal.CountAsync(filter, includes);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Count), new { data = "" });
            throw;
        }
    }
}