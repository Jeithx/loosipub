using Core.Constants;
using Core.Enums;
using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.File;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Claims;

namespace Api.Controllers.v1.PostController;

public partial class PostController
{
    [Authorize(Roles = "ContentCreator")]
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] PostWD model)
    {
        var claims = _tokenHelper.GetClaimsFromToken(HttpContext);
        if (claims == null)
            return Unauthorized(Messages.UnAuthorize);

        var userId = claims.FindFirst(ClaimTypes.NameIdentifier).Value;

        model.UserId = long.Parse(userId);
        var resultModel = await _service.Create(model);

        if (!resultModel.Success)
            return BadRequest(new ErrorDataResult<PostRD>(resultModel.Message));

        var postMedias = new List<PostMediaRD>();
        if (model.MediaType != (int)EPostMediaType.Text)
        {
            foreach (var item in model.PostMedias)
            {
                if (item.File != null && item.Type == (int)EPostMediaType.Image)
                {
                    string blurredImage = null;
                    if (model.IsNsfw)
                    {
                        var blurredFormFile = await ImageBlurHelper.ApplyBlurAndReturnFormFileAsync(item.File, 40f);
                        blurredImage = await _fileManagerHelper.UploadIFormFileAsync(blurredFormFile,
                            $"Loosip/Post/{resultModel.Data.Id.ToString()}/Blurred");
                    }

                    var image = await _fileManagerHelper.UploadIFormFileAsync(item.File,
                        $"Loosip/Post/{resultModel.Data.Id.ToString()}");

                    var result = await _postMediaService.Create(new PostMediaWD
                    {
                        PostId = resultModel.Data.Id,
                        IsActive = true,
                        CreationDate = DateTime.Now,
                        Order = item.Order,
                        Type = item.Type,
                        MediaUrl = image,
                        ThumbnailUrl = blurredImage
                    });

                    postMedias.Add(result.Data);
                }
                else if (item.File != null && item.Type == (int)EPostMediaType.Video)
                {
                    var video = await _fileManagerHelper.UploadIFormFileAsync(item.File,
                      $"Loosip/Post/{resultModel.Data.Id.ToString()}");

                    var result = await _postMediaService.Create(new PostMediaWD
                    {
                        PostId = resultModel.Data.Id,
                        IsActive = true,
                        CreationDate = DateTime.Now,
                        Order = item.Order,
                        Type = item.Type,
                        MediaUrl = video,
                        ThumbnailUrl = null
                    });

                    postMedias.Add(result.Data);
                }
            }
        }

        var postTagsRD = new List<PostTagRD>();
        foreach (var item in model.PostTags)
        {
            var result = await _postTagService.Create(new PostTagWD { PostId = resultModel.Data.Id, Tag = item.Tag });
            postTagsRD.Add(result.Data);
        }

        if (resultModel.Success)
        {
            resultModel.Data.PostMedia = postMedias;
            resultModel.Data.PostTags = postTagsRD;
            return Ok(new SuccessDataResult<PostRD>(resultModel.Data, resultModel.Message));
        }
        else
            return BadRequest(new ErrorDataResult<PostRD>(resultModel.Message));
    }
}