using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.PostMediaController;

[Route("v1/api/[controller]")]
[ApiController]
public partial class PostMediaController : ControllerBase
{
    private readonly IPostMediaService _service;
    public PostMediaController(IPostMediaService service)
    {
        _service = service;
    }
}
