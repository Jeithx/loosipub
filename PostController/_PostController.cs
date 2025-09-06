using Business.Abstract;
using Core.Helper;
using Core.Utilities.Security.Jwt;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.PostController;

[Route("v1/api/[controller]")]
[ApiController]
public partial class PostController : ControllerBase
{
    private readonly IPostService _service;
    readonly ITokenHelper _tokenHelper;
    readonly IPostMediaService _postMediaService;
    readonly IPostTagService _postTagService;
    readonly FileManagerHelper _fileManagerHelper;
    readonly IConfiguration _configuration;

    public PostController(IPostService service, ITokenHelper tokenHelper, IPostMediaService postMediaService, IPostTagService postTagService, IConfiguration configuration)
    {
        _service = service;
        _tokenHelper = tokenHelper;
        _postMediaService = postMediaService;
        _postTagService = postTagService;
        _configuration = configuration;

        _fileManagerHelper = new FileManagerHelper(_configuration);
    }
}
