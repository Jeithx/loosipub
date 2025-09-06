using Business.Abstract;
using Core.Utilities.Security.Jwt;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.UserFollowerController;

[Route("v1/api/[controller]")]
[ApiController]
public partial class UserFollowerController : ControllerBase
{
    private readonly IUserFollowerService _service;
    readonly ITokenHelper _tokenHelper;

    public UserFollowerController(IUserFollowerService service, ITokenHelper tokenHelper)
    {
        _service = service;
        _tokenHelper = tokenHelper;
    }
}
