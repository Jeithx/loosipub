using Business.Abstract;
using Core.Utilities.Security.Jwt;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.AuthController;

[Route("v1/api/[controller]")]
[ApiController]
public partial class AuthController : ControllerBase
{
    private readonly IAuthService _service;
    readonly ITokenHelper _tokenHelper;
    public AuthController(IAuthService service, ITokenHelper tokenHelper)
    {
        _service = service;
        _tokenHelper = tokenHelper;
    }
}
