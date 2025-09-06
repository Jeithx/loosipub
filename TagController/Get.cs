using Core.Models.Read;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.TagController
{
    public partial class TagController
    {
        [HttpGet]
        public async Task<IActionResult> Get(int? pageNumber = 1, int? pageSize = 10)
        {
            var result = await _service.Get(x => x.IsActive, null, pageNumber, pageSize);

            return Ok(new SuccessDataResult<List<TagRD>>(result.Data, result.Message, result.RecordTotals));
        }
    }
}
