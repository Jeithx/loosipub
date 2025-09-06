using Core.Models.Read;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.FaqController
{
    public partial class FaqController
    {
        [HttpGet]
        public async Task<IActionResult> Get(int typeId, long? languageId = 1, int? pageNumber = 1, int? pageSize = 10)
        {
            var result = await _service.Get(x => (languageId == 1 || x.LanguageId == languageId) && x.TypeId == typeId, null, pageNumber, pageSize);

            return Ok(new SuccessDataResult<List<FaqRD>>(result.Data, result.Message, result.RecordTotals));
        }
    }
}
