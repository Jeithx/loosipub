using Core.Models.Read;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.CityController
{
    public partial class CityController
    {
        [HttpGet]
        public async Task<IActionResult> Get(long countryId, int? pageNumber = 1, int? pageSize = 10,bool notPagination=false)
        {
            var result = await _service.Get(x => x.CountryId == countryId && x.IsActive, null, pageNumber, pageSize, notPagination);

            return Ok(new SuccessDataResult<List<CityRD>>(result.Data, result.Message, result.RecordTotals));
        }
    }
}
