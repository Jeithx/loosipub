using AutoMapper;
using Business.Abstract;
using Core.Constants;
using Core.Enums;
using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using CoreXNugetPackage.DataAccess.Dapper;
using Dapper;
using DataAccess.Abstract;
using Entities.Models;
using System.Linq.Expressions;
namespace Business.Concrete;
public class PostService : IPostService
{
    private readonly IMapper _mapper;
    readonly ILogger _logger;
    IPostDAL _postDal;
    readonly IPostLikeDAL _postLikeDal;
    readonly IUserFollowerDAL _userFollowerDal;
    readonly INotificationDAL _notificationDal;
    readonly DapperConnectionHelper _dapperConnectionHelper;

    public PostService(IPostDAL postDal, IMapper mapper, ILogger logger, IPostLikeDAL postLikeDal, INotificationDAL notificationDal, IUserFollowerDAL userFollowerDal, DapperConnectionHelper dapperConnectionHelper)
    {
        _postDal = postDal;
        _mapper = mapper;
        _logger = logger;
        _postLikeDal = postLikeDal;
        _notificationDal = notificationDal;
        _userFollowerDal = userFollowerDal;
        _dapperConnectionHelper = dapperConnectionHelper;
    }

    public async Task<IDataResult<PostRD>> Create(PostWD postWD)
    {
        try
        {
            var data = await _postDal.AddAsync(_mapper.Map<Post>(postWD));
            if (data != null)
            {
                //TODO: Rabbit mq servisine eklenecek.
                var followers = await _userFollowerDal.GetListAsync(x => x.FollowedId == postWD.UserId && x.IsActive);
                for (int i = 0; i < followers.Count; i++)
                {
                    await _notificationDal.AddAsync(new Notification
                    {
                        UserId = followers[i].FollowerId,
                        Content = $"shared a new post.",
                        CreationDate = DateTime.Now,
                        IsActive = true,
                        IsRead = false,
                        Type = (int)ENotificationType.NewPost,
                        UpdateDate = DateTime.Now
                    });
                }
            }

            return new SuccessDataResult<PostRD>(_mapper.Map<PostRD>(data), Messages.AddingProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Create), new { data = postWD });
            return new ErrorDataResult<PostRD>(Messages.AddingProcessFailed);
        }
    }

    public async Task<bool> Delete(long id, long userId, bool? isJob = false)
    {
        try
        {
            if (isJob == true)
            {
                var post = await _postDal.GetAsync(x => x.Id == id);
                if (post != null)
                {
                    post.IsActive = false;
                    await _postDal.UpdateAsync(post);
                }
            }
            else
            {
                var post = await _postDal.GetAsync(x => x.Id == id && x.UserId == userId && x.IsActive);
                if (post != null)
                {
                    post.IsActive = false;
                    await _postDal.UpdateAsync(post);
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

    public async Task<IDataResult<List<PostRD>>> Get(Expression<Func<Entities.Models.Post, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10, long? userId = 0)
    {
        try
        {
            var posts = await _postDal.GetPageAsync(filter, x => x.CreationDate, null, includes, (int)pageNumber!, (int)pageSize!);
            var mappedData = _mapper.Map<List<PostRD>>(posts);
            if (pageSize == 10 && userId != 0)
            {
                for (int i = 0; i < mappedData.Count; i++)
                {
                    var data = await _postLikeDal.GetAsync(x => x.PostId == mappedData[i].Id && x.UserId == userId);
                    if (data != null)
                    {
                        mappedData[i].CurrentUserLiked = true;
                    }
                }
            }

            var recordTotal = await _postDal.CountAsync(filter);
            return new SuccessDataResult<List<PostRD>>(mappedData, Messages.ProcessSuccess, recordTotal);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Get), new { data = filter });
            throw new Exception(Messages.GetProcessFailed, ex);
        }
    }

    public async Task<IDataResult<List<PostRD>>> GetByRandom(int? pageNumber = 1, int? pageSize = 10, long? userId = 0)
    {
        try
        {
            var query =
               $@"SELECT Id FROM Posts
                  WHERE IsActive = 1  
                  ORDER BY NEWID()      
                  OFFSET ({pageNumber} - 1) * {pageSize} ROWS
                  FETCH NEXT {pageSize} ROWS ONLY;";

            using var connection = _dapperConnectionHelper.CreateSqlConnection();
            var datas = connection.Query<long>(query).ToList();

            var getDatas = await _postDal.GetListAsync(x => datas.Contains(x.Id), new[] { "PostMedia", "PostTags", "User" });

            var mappedData = _mapper.Map<List<PostRD>>(getDatas);
            if (pageSize == 10 && userId != 0)
            {
                for (int i = 0; i < mappedData.Count; i++)
                {
                    var data = await _postLikeDal.GetAsync(x => x.PostId == mappedData[i].Id && x.UserId == userId);
                    if (data != null)
                    {
                        mappedData[i].CurrentUserLiked = true;
                    }
                }
            }

            return new SuccessDataResult<List<PostRD>>(mappedData, Messages.ProcessSuccess, await _postDal.CountAsync(x => x.IsActive));
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Get), new { data = pageNumber });
            throw new Exception(Messages.GetProcessFailed, ex);
        }
    }

    public async Task<IDataResult<List<PostRD>>> GetByFollowed(long userId, int? pageNumber = 1, int? pageSize = 10)
    {
        try
        {
            var query =
               $@"select p.Id from Posts p 
                  left join Users u on u.Id = p.UserId
                  left join UserFollowers uf on uf.FollowedId = u.Id
                  where uf.FollowerId = {userId}
                  order by p.CreationDate desc
                  OFFSET ({pageNumber} - 1) * {pageSize} ROWS
                  FETCH NEXT {pageSize} ROWS ONLY;";

            using var connection = _dapperConnectionHelper.CreateSqlConnection();
            var datas = connection.Query<long>(query).ToList();

            var getDatas = await _postDal.GetListAsync(x => datas.Contains(x.Id), new[] { "PostMedia", "PostTags", "User" });
            var mappedData = _mapper.Map<List<PostRD>>(getDatas);
            if (pageSize == 10 && userId != 0)
            {
                for (int i = 0; i < mappedData.Count; i++)
                {
                    var data = await _postLikeDal.GetAsync(x => x.PostId == mappedData[i].Id && x.UserId == userId);
                    if (data != null)
                    {
                        mappedData[i].CurrentUserLiked = true;
                    }
                }
            }

            return new SuccessDataResult<List<PostRD>>(mappedData, Messages.ProcessSuccess, await _postDal.CountAsync(x => x.IsActive));
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Get), new { data = pageNumber });
            throw new Exception(Messages.GetProcessFailed, ex);
        }
    }

    public async Task<PostRD?> GetByExpression(Expression<Func<Entities.Models.Post, bool>> filter = null, string[] includes = null)
    {
        try
        {
            var result = await _postDal.GetAsync(filter, includes);
            if (result != null)
                return _mapper.Map<PostRD>(result);

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(GetByExpression), new { data = filter });
            throw new Exception(Messages.ProcessFailed, ex);
        }
    }

    public async Task<IDataResult<PostRD>> Update(PostUpdateWD postWD)
    {
        try
        {
            await _postDal.UpdateAsync(_mapper.Map<Post>(postWD));
            return new SuccessDataResult<PostRD>(_mapper.Map<PostRD>(postWD), Messages.UpdateProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Update), new { data = postWD });
            return new ErrorDataResult<PostRD>(Messages.UpdateProcessFailed);
        }
    }

    public async Task<long> Count(Expression<Func<Post, bool>>? filter = null, string[]? includes = null)
    {
        try
        {
            return await _postDal.CountAsync(filter, includes);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Count), new { data = "" });
            throw;
        }
    }
}