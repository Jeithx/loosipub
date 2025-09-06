using AutoMapper;
using Business.Abstract;
using Core.Constants;
using Core.Enums;
using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Models;
using System.Linq.Expressions;
namespace Business.Concrete;
public class PostCommentService : IPostCommentService
{
    private readonly IMapper _mapper;
    readonly ILogger _logger;
    IPostCommentDAL _postcommentDal;
    readonly IPostService _postService;
    readonly INotificationService _notificationService;
    public PostCommentService(IPostCommentDAL postcommentDal, IMapper mapper, ILogger logger, IPostService postService, INotificationService notificationService)
    {
        _postcommentDal = postcommentDal;
        _mapper = mapper;
        _logger = logger;
        _postService = postService;
        _notificationService = notificationService;
    }

    public async Task<IDataResult<PostCommentRD>> Create(PostCommentWD postcommentWD)
    {
        try
        {
            var data = await _postcommentDal.AddAsync(_mapper.Map<PostComment>(postcommentWD));
            var post = await _postService.GetByExpression(x => x.Id == postcommentWD.PostId);
            if (post != null)
            {
                post.CommentCount += 1;
                await _postService.Update(_mapper.Map<PostUpdateWD>(post));

                await _notificationService.Create(new NotificationWD
                {
                    UserId = post.UserId,
                    Content = $"A new comment was made on your post",
                    CreationDate = DateTime.Now,
                    IsActive = true,
                    IsRead = false,
                    Type = (int)ENotificationType.NewComment,
                    UpdateDate = DateTime.Now
                });
            }

            return new SuccessDataResult<PostCommentRD>(_mapper.Map<PostCommentRD>(data), Messages.AddingProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Create), new { data = postcommentWD });
            return new ErrorDataResult<PostCommentRD>(Messages.AddingProcessFailed);
        }
    }

    public async Task<bool> Delete(long id, long userId)
    {
        try
        {
            PostComment postcomment = await _postcommentDal.GetAsync(x => x.Id == id && x.UserId == userId);
            if (postcomment != null)
            {
                await _postcommentDal.DeleteAsync(postcomment);
                var post = await _postService.GetByExpression(x => x.Id == postcomment.PostId);
                if (post != null)
                {
                    post.CommentCount -= 1;
                    if (post.CommentCount < 0)
                        post.CommentCount = 0; // Ensure comment count does not go negative"
                    await _postService.Update(_mapper.Map<PostUpdateWD>(post));
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Delete), new { data = id });
            throw;
        }
    }

    public async Task<IDataResult<List<PostCommentRD>>> Get(Expression<Func<Entities.Models.PostComment, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10)
    {
        try
        {
            var postcomments = await _postcommentDal.GetPageAsync(filter, includes, (int)pageNumber!, (int)pageSize!);
            return new SuccessDataResult<List<PostCommentRD>>(_mapper.Map<List<PostCommentRD>>(postcomments), Messages.ProcessSuccess, await _postcommentDal.CountAsync(filter));
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Get), new { data = filter });
            throw new Exception(Messages.GetProcessFailed, ex);
        }
    }

    public async Task<PostCommentRD?> GetByExpression(Expression<Func<Entities.Models.PostComment, bool>> filter = null, string[] includes = null)
    {
        try
        {
            var result = await _postcommentDal.GetAsync(filter, includes);
            if (result != null)
                return _mapper.Map<PostCommentRD>(result);

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(GetByExpression), new { data = filter });
            throw new Exception(Messages.ProcessFailed, ex);
        }
    }

    public async Task<IDataResult<PostCommentRD>> Update(PostCommentWD postcommentWD)
    {
        try
        {
            await _postcommentDal.UpdateAsync(_mapper.Map<PostComment>(postcommentWD));
            return new SuccessDataResult<PostCommentRD>(_mapper.Map<PostCommentRD>(postcommentWD), Messages.UpdateProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Update), new { data = postcommentWD });
            return new ErrorDataResult<PostCommentRD>(Messages.UpdateProcessFailed);
        }
    }

    public async Task<long> Count(Expression<Func<PostComment, bool>>? filter = null, string[]? includes = null)
    {
        try
        {
            return await _postcommentDal.CountAsync(filter, includes);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Count), new { data = "" });
            throw;
        }
    }
}