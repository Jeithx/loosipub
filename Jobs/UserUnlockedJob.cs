using DataAccess.Abstract;
using Quartz;

namespace API.Jobs
{
    public class UserUnlockedJob : IJob
    {
        readonly IUserDAL _userService;
        readonly IConfiguration _configuration;
        private bool isLocal = true;

        public UserUnlockedJob(IUserDAL userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
            isLocal = bool.Parse(_configuration["IsLocal"] ?? "true");
        }

        public async Task Execute(IJobExecutionContext context)
        {
            if (!isLocal)
                await Run();
        }

        private async Task Run()
        {
            var users = await _userService.GetListAsync(x => x.IsActive == true && x.IsLocked == true);
            if (users != null && users.Count > 0)
            {
                foreach (var user in users)
                {
                    user.IsLocked = false;
                    await _userService.UpdateAsync(user);
                }
            }
        }
    }
}
