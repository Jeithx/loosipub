using Business.Abstract;
using Core.Helper;
using Core.Utilities.Security.Jwt;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers.v1.UserController
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public partial class UserController : ControllerBase
    {
        private readonly IUserService _service;
        readonly ITokenHelper _tokenHelper;
        readonly FileManagerHelper _fileManagerHelper;
        readonly IConfiguration _configuration;
        readonly IUserFollowerService _userFollowerService;

        public UserController(IUserService service, ITokenHelper tokenHelper, IConfiguration configuration, IUserFollowerService userFollowerService)
        {
            _service = service;
            _tokenHelper = tokenHelper;
            _configuration = configuration;
            _fileManagerHelper = new FileManagerHelper(_configuration);
            _userFollowerService = userFollowerService;
        }
    }
}
