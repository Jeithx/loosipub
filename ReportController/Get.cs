using Core.Models.Read;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.ReportController
{
    public partial class ReportController
    {
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Get(int? pageNumber = 1, int? pageSize = 10)
        {
            var result = await _service.Get(x => x.IsActive, null, pageNumber, pageSize);

            return Ok(new SuccessDataResult<List<ReportRD>>(result.Data, result.Message, result.RecordTotals));
        }
    }
}
