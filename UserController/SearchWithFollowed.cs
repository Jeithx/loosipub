using Core.Models.Read;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers.v1.UserController
{
    public partial class UserController
    {
        /// <summary>
        /// Takip ettikleri içinde arama yapar
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet("SearchWithFollowed")]
        public async Task<IActionResult> SearchWithFollowed(long userId, string username)
        {
            var result = await _service.SearchWithFollowed(userId, username);
            return Ok(new SuccessDataResult<List<UserRD?>>(result));
        }
    }
}
