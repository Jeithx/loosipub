using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using Entities.Dtos;
using System.Linq.Expressions;

namespace Business.Abstract;

public interface IMessageService
{
    Task<IDataResult<MessageRD>> Create(MessageWD model);
    Task<IDataResult<MessageRD>> Update(MessageWD model);
    Task<bool> Delete(long id);
    Task<IDataResult<List<MessageRD>>> Get(Expression<Func<Entities.Models.Message, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10);
    Task<MessageRD> GetByExpression(Expression<Func<Entities.Models.Message, bool>> filter = null, string[]? includes = null);
    Task<long> Count(Expression<Func<Entities.Models.Message, bool>>? filter = null, string[]? includes = null);
    Task<IDataResult<List<DapperWithGetMessage>>> GetGroupedMessages(long userId, int? pageNumber = 1, int? pageSize = 10);
}