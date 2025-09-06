using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.PostTagController;

[Route("v1/api/[controller]")]
[ApiController]
public partial class PostTagController : ControllerBase
{
    private readonly IPostTagService _service;
    public PostTagController(IPostTagService service)
    {
        _service = service;
    }
}
