using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.GenderController;

[Route("v1/api/[controller]")]
[ApiController]
public partial class GenderController : ControllerBase
{
    private readonly IGenderService _service;
    public GenderController(IGenderService service)
    {
        _service = service;
    }
}
