using Business.Abstract;
using Core.Utilities.Security.Jwt;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.SubscriberController;

[Route("v1/api/[controller]")]
[ApiController]
public partial class SubscriberController : ControllerBase
{
    private readonly ISubscriberService _service;
    readonly ITokenHelper _tokenHelper;
    public SubscriberController(ISubscriberService service, ITokenHelper tokenHelper)
    {
        _service = service;
        _tokenHelper = tokenHelper;
    }
}
