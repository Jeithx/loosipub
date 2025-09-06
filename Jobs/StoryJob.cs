using Business.Abstract;
using Core.Enums;
using Quartz;

namespace API.Jobs
{
    public class StoryJob : IJob
    {
        readonly IPostService _postService;
        readonly IConfiguration _configuration;
        private bool isLocal = true;


        public StoryJob(IPostService postService, IConfiguration configuration)
        {
            _postService = postService;
            _configuration = configuration;
            isLocal = bool.Parse(_configuration["IsLocal"] ?? "true");
        }

        public async Task Execute(IJobExecutionContext context)
        {
            if (!isLocal)
                await StoryRun();
        }

        private async Task StoryRun()
        {
            var posts = await _postService.Get(x => x.IsActive && x.Type == (int)EPostType.Story);
            if (posts.Data != null && posts.Data.Count > 0)
            {
                foreach (var post in posts.Data)
                {
                    if (post.CreationDate > post.CreationDate.AddHours(24))
                    {
                        await _postService.Delete(post.Id, 0, true);
                    }
                }
            }
        }
    }
}
