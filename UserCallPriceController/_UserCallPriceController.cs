using Business.Abstract;
using Core.Utilities.Security.Jwt;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.UserCallPriceController;

[Route("v1/api/[controller]")]
[ApiController]
public partial class UserCallPriceController : ControllerBase
{
    private readonly IUserCallPriceService _service;
    readonly ITokenHelper _tokenHelper;

    public UserCallPriceController(IUserCallPriceService service, ITokenHelper tokenHelper)
    {
        _service = service;
        _tokenHelper = tokenHelper;
    }
}
