using Core.Models.Read;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers.v1.UserController
{
    public partial class UserController
    {
        [HttpGet("Search")]
        public async Task<IActionResult> Search(string username)
        {
            var result = await _service.GetByExpression(x => x.UserName.ToLower().Contains(username.ToLower()) && x.IsActive);
            return Ok(new SuccessDataResult<UserRD>(result));
        }
    }
}
