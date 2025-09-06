using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.TagController;

[Route("v1/api/[controller]")]
[ApiController]
public partial class TagController : ControllerBase
{
    private readonly ITagService _service;
    public TagController(ITagService service)
    {
        _service = service;
    }
}
