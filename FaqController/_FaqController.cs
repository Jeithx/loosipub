using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.FaqController;

[Route("v1/api/[controller]")]
[ApiController]
public partial class FaqController : ControllerBase
{
    private readonly IFaqService _service;
    public FaqController(IFaqService service)
    {
        _service = service;
    }
}
