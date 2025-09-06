using Business.Abstract;
using Core.Helper;
using Core.Utilities.Security.Jwt;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.MessageController;

[Route("v1/api/[controller]")]
[ApiController]
public partial class MessageController : ControllerBase
{
    private readonly IMessageService _service;
    readonly ITokenHelper _tokenHelper;
    readonly FileManagerHelper _fileManagerHelper;
    readonly IConfiguration _configuration;
    public MessageController(IMessageService service, ITokenHelper tokenHelper , IConfiguration configuration)
    {
        _service = service;
        _tokenHelper = tokenHelper;
        _configuration = configuration;
        _fileManagerHelper = new FileManagerHelper(_configuration);
    }
}
