using Core.Models.Read;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.UserCallPriceController
{
    public partial class UserCallPriceController
    {
        [Authorize(Roles = "ContentCreator, Fan")]
        [HttpGet]
        public async Task<IActionResult> Get(int? pageNumber = 1, int? pageSize = 10)
        {
            var result = await _service.Get(x => x.IsActive, new[] { "User" }, pageNumber, pageSize);

            return Ok(new SuccessDataResult<List<UserCallPriceRD>>(result.Data, result.Message, result.RecordTotals));
        }
    }
}
