using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.LanguageController;

[Route("v1/api/[controller]")]
[ApiController]
public partial class LanguageController : ControllerBase
{
    private readonly ILanguageService _service;
    public LanguageController(ILanguageService service)
    {
        _service = service;
    }
}
