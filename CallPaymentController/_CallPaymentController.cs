using Business.Abstract;
using Core.Utilities.Security.Jwt;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.CallPaymentController;

[Route("v1/api/[controller]")]
[ApiController]
public partial class CallPaymentController : ControllerBase
{
    private readonly ICallPaymentService _service;
    readonly ITokenHelper _tokenHelper;
    public CallPaymentController(ICallPaymentService service, ITokenHelper tokenHelper)
    {
        _service = service;
        _tokenHelper = tokenHelper;
    }
}
