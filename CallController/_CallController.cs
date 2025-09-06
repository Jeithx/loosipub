using Business.Abstract;
using Core.Utilities.Security.Jwt;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.CallController;

[Route("v1/api/[controller]")]
[ApiController]
public partial class CallController : ControllerBase
{
    private readonly ICallService _service;
    readonly ITokenHelper _tokenHelper;

    public CallController(ICallService service, ITokenHelper tokenHelper)
    {
        _service = service;
        _tokenHelper = tokenHelper;
    }
}
