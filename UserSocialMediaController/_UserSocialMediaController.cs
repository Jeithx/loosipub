using Business.Abstract;
using Core.Utilities.Security.Jwt;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.UserSocialMediaController;

[Route("v1/api/[controller]")]
[ApiController]
public partial class UserSocialMediaController : ControllerBase
{
    private readonly IUserSocialMediaService _service;
    readonly ITokenHelper _tokenHelper;
    public UserSocialMediaController(IUserSocialMediaService service, ITokenHelper tokenHelper)
    {
        _service = service;
        _tokenHelper = tokenHelper;
    }
}
