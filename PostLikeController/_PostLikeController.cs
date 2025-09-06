using Business.Abstract;
using Core.Utilities.Security.Jwt;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.PostLikeController;

[Route("v1/api/[controller]")]
[ApiController]
public partial class PostLikeController : ControllerBase
{
    private readonly IPostLikeService _service;
    readonly ITokenHelper _tokenHelper;
    public PostLikeController(IPostLikeService service, ITokenHelper tokenHelper)
    {
        _service = service;
        _tokenHelper = tokenHelper;
    }
}
