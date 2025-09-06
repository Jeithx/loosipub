using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.CountryController;

[Route("v1/api/[controller]")]
[ApiController]
public partial class CountryController : ControllerBase
{
    private readonly ICountryService _service;
    public CountryController(ICountryService service)
    {
        _service = service;
    }
}
