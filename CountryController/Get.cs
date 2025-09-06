using Core.Models.Read;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.CountryController
{
    public partial class CountryController
    {
        [HttpGet]
        public async Task<IActionResult> Get(long? languageId = 1, int? pageNumber = 1, int? pageSize = 10,bool notPagination=false)
        {
            var result = await _service.Get(x => (languageId == 1 || x.LanguageId == languageId) && x.IsActive, null, pageNumber, pageSize, notPagination);

            return Ok(new SuccessDataResult<List<CountryRD>>(result.Data, result.Message, result.RecordTotals));
        }
    }
}
