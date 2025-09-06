using Core.Models.Read;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.SocialMediaController
{
    public partial class SocialMediaController
    {
        [HttpGet]
        public async Task<IActionResult> Get(long? languageId = 1, int? pageNumber = 1, int? pageSize = 10)
        {
            var result = await _service.Get(x => x.IsActive && (languageId == 1 || x.LanguageId == languageId), null, pageNumber, pageSize);

            return Ok(new SuccessDataResult<List<SocialMediaRD>>(result.Data, result.Message, result.RecordTotals));
        }
    }
}
