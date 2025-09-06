using Business.Abstract;
using Core.Utilities.Security.Jwt;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.ReportController;

[Route("v1/api/[controller]")]
[ApiController]
public partial class ReportController : ControllerBase
{
    private readonly IReportService _service;
    readonly ITokenHelper _tokenHelper;
    public ReportController(IReportService service, ITokenHelper tokenHelper)
    {
        _service = service;
        _tokenHelper = tokenHelper;
    }
}
