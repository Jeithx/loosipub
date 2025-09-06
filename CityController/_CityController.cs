using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.CityController;

[Route("v1/api/[controller]")]
[ApiController]
public partial class CityController : ControllerBase
{
    private readonly ICityService _service;
    public CityController(ICityService service)
    {
        _service = service;
    }
}
