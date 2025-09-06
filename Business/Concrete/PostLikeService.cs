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
public class PostLikeService : IPostLikeService
{
    private readonly IMapper _mapper;
    readonly ILogger _logger;
    IPostLikeDAL _postlikeDal;
    readonly IPostService _postService;
    readonly INotificationService _notificationService;
    readonly IUserFollowerDAL _userFollowerDal;

    public PostLikeService(IPostLikeDAL postlikeDal, IMapper mapper, ILogger logger, IPostService postService, INotificationService notificationService, IUserFollowerDAL userFollowerDal)
    {
        _postlikeDal = postlikeDal;
        _mapper = mapper;
        _logger = logger;
        _postService = postService;
        _notificationService = notificationService;
        _userFollowerDal = userFollowerDal;
    }

    public async Task<IDataResult<PostLikeRD>> Create(PostLikeWD postlikeWD)
    {
        try
        {
            var data = await _postlikeDal.AddAsync(_mapper.Map<PostLike>(postlikeWD));
            var post = await _postService.GetByExpression(x => x.Id == postlikeWD.PostId && x.IsActive);
            if (post != null)
            {
                post.LikeCount += 1;
                await _postService.Update(_mapper.Map<PostUpdateWD>(post));

                await _notificationService.Create(new NotificationWD
                {
                    UserId = post.UserId,
                    Content = $"A new like was made on your post",
                    CreationDate = DateTime.Now,
                    IsActive = true,
                    IsRead = false,
                    Type = (int)ENotificationType.NewLike,
                    UpdateDate = DateTime.Now
                });
            }

            return new SuccessDataResult<PostLikeRD>(_mapper.Map<PostLikeRD>(data), Messages.AddingProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Create), new { data = postlikeWD });
            return new ErrorDataResult<PostLikeRD>(Messages.AddingProcessFailed);
        }
    }

    public async Task<bool> Delete(long id, long userId)
    {
        try
        {
            PostLike postlike = await _postlikeDal.GetAsync(x => x.PostId == id && x.UserId == userId && x.IsActive);
            if (postlike != null)
            {
                await _postlikeDal.DeleteAsync(postlike);

                var post = await _postService.GetByExpression(x => x.Id == postlike.PostId);
                post.LikeCount -= 1;
                if (post.LikeCount < 0)
                    post.LikeCount = 0;
                await _postService.Update(_mapper.Map<PostUpdateWD>(post));
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Delete), new { data = id });
            throw;
        }
    }

    public async Task<IDataResult<List<PostLikeRD>>> Get(Expression<Func<Entities.Models.PostLike, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10, bool? isFollowerCheck = false, long? currentUserId = 0)
    {
        try
        {
            var postlikes = await _postlikeDal.GetPageAsync(filter, x => x.CreationDate, null, includes, (int)pageNumber!, (int)pageSize!);
            var mappedData = _mapper.Map<List<PostLikeRD>>(postlikes);
            if (isFollowerCheck == true && currentUserId != 0)
            {
                foreach (var item in mappedData)
                {
                    item.FollowedUserCheck = await _userFollowerDal.AnyAsync(x => x.FollowerId == currentUserId && x.FollowedId == item.UserId && x.IsActive);
                }
            }

            return new SuccessDataResult<List<PostLikeRD>>(mappedData, Messages.ProcessSuccess, await _postlikeDal.CountAsync(filter));
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Get), new { data = filter });
            throw new Exception(Messages.GetProcessFailed, ex);
        }
    }

    public async Task<PostLikeRD?> GetByExpression(Expression<Func<Entities.Models.PostLike, bool>> filter = null, string[] includes = null)
    {
        try
        {
            var result = await _postlikeDal.GetAsync(filter, includes);
            if (result != null)
                return _mapper.Map<PostLikeRD>(result);

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(GetByExpression), new { data = filter });
            throw new Exception(Messages.ProcessFailed, ex);
        }
    }

    public async Task<IDataResult<PostLikeRD>> Update(PostLikeWD postlikeWD)
    {
        try
        {
            await _postlikeDal.UpdateAsync(_mapper.Map<PostLike>(postlikeWD));
            return new SuccessDataResult<PostLikeRD>(_mapper.Map<PostLikeRD>(postlikeWD), Messages.UpdateProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Update), new { data = postlikeWD });
            return new ErrorDataResult<PostLikeRD>(Messages.UpdateProcessFailed);
        }
    }

    public async Task<long> Count(Expression<Func<PostLike, bool>>? filter = null, string[]? includes = null)
    {
        try
        {
            return await _postlikeDal.CountAsync(filter, includes);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Count), new { data = "" });
            throw;
        }
    }
}