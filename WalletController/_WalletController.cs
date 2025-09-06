using Microsoft.AspNetCore.Mvc;
using Business.Abstract;
using Core.Utilities.Security.Jwt;

namespace Api.Controllers.v1.WalletController;

[Route("v1/api/[controller]")]
[ApiController]
public partial class WalletController : ControllerBase
{
    private readonly IWalletService _service;
    private readonly ITokenHelper _tokenHelper;

    public WalletController(IWalletService service, ITokenHelper tokenHelper)
    {
        _service = service;
        _tokenHelper = tokenHelper;
    }
}