using Core.Models.Read;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers.v1.UserController
{
    public partial class UserController
    {
        /// <summary>
        /// Takip edenler içinde arama yapar
        /// </summary>
        /// <param name="followedUserId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet("SearchWithFollower")]
        public async Task<IActionResult> SearchWithFollower(long followedUserId, string username)
        {
            var result = await _service.SearchWithFollower(followedUserId, username);
            return Ok(new SuccessDataResult<List<UserRD?>>(result));
        }
    }
}
