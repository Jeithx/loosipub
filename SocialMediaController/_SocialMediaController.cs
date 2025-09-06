using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.SocialMediaController;

[Route("v1/api/[controller]")]
[ApiController]
public partial class SocialMediaController : ControllerBase
{
    private readonly ISocialMediaService _service;
    public SocialMediaController(ISocialMediaService service)
    {
        _service = service;
    }
}
