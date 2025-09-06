using Core.Constants;
using Core.Helper;
using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Claims;

namespace Api.Controllers.v1.MessageController;

public partial class MessageController
{
    [Authorize(Roles = "ContentCreator, Fan")]
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] MessageWD model)
    {
        var claims = _tokenHelper.GetClaimsFromToken(HttpContext);
        if (claims == null)
            return Unauthorized(Messages.UnAuthorize);

        var currentUserId = claims.FindFirst(ClaimTypes.NameIdentifier).Value;

        model.SenderId = long.Parse(currentUserId);

        var randomGuid = Guid.NewGuid().ToString();

        if (model.File != null)
        {
            var image = await _fileManagerHelper.UploadIFormFileAsync(model.File,
                       $"Loosip/Messages/{randomGuid}");
            model.FileUrl = image;
        }


        var resultModel = await _service.Create(model);

        if (resultModel.Success)
            return Ok(new SuccessDataResult<MessageRD>(resultModel.Data, resultModel.Message));
        else
            return BadRequest(new ErrorDataResult<MessageRD>(resultModel.Message));
    }
}
