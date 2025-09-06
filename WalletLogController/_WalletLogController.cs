using Microsoft.AspNetCore.Mvc;
using Business.Abstract;
using Core.Utilities.Security.Jwt;

namespace Api.Controllers.v1.WalletLogController;

[Route("v1/api/[controller]")]
[ApiController]
public partial class WalletLogController : ControllerBase
{
    private readonly IWalletLogService _service;
    private readonly ITokenHelper _tokenHelper;

    public WalletLogController(IWalletLogService service, ITokenHelper tokenHelper)
    {
        _service = service;
        _tokenHelper = tokenHelper;
    }
}