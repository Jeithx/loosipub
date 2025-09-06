using Business.Abstract;
using Core.Utilities.Security.Jwt;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.UserSubscriptionPlanController;

[Route("v1/api/[controller]")]
[ApiController]
public partial class UserSubscriptionPlanController : ControllerBase
{
    private readonly IUserSubscriptionPlanService _service;
    readonly ITokenHelper _tokenHelper;
    public UserSubscriptionPlanController(IUserSubscriptionPlanService service, ITokenHelper tokenHelper)
    {
        _service = service;
        _tokenHelper = tokenHelper;
    }
}
