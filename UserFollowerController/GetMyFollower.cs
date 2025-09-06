using Core.Constants;
using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Claims;

namespace Api.Controllers.v1.UserFollowerController
{
    public partial class UserFollowerController
    {
        [HttpGet("GetMyFollower")]
        public async Task<IActionResult> GetMyFollower(long userId, int? pageNumber = 1, int? pageSize = 10)
        {
          
            var result = await _service.Get(x => x.IsActive && x.FollowedId == userId, new[] { "Follower" }, pageNumber, pageSize);

            return Ok(new SuccessDataResult<List<UserFollowerRD>>(result.Data, result.Message, result.RecordTotals));
        }
    }
}
