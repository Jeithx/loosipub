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
        [HttpGet("GetMyFollowed")]
        public async Task<IActionResult> GetMyFollowed(long userId, int? pageNumber = 1, int? pageSize = 10)
        {
        
            var result = await _service.Get(x => x.IsActive && x.FollowerId == userId, new[] { "Followed" }, pageNumber, pageSize);

            return Ok(new SuccessDataResult<List<UserFollowerRD>>(result.Data, result.Message, result.RecordTotals));
        }
    }
}
