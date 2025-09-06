using Business.Abstract;
using Core.Utilities.Security.Jwt;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.BlockedUserController;

[Route("v1/api/[controller]")]
[ApiController]
public partial class BlockedUserController : ControllerBase
{
    private readonly IBlockedUserService _service;
    readonly ITokenHelper _tokenHelper;
    public BlockedUserController(IBlockedUserService service, ITokenHelper tokenHelper)
    {
        _service = service;
        _tokenHelper = tokenHelper;
    }
}
