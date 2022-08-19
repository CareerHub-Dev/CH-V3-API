using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;

namespace WebUI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApiControllerBase : ControllerBase
{
    public AccountInfo? AccountInfo => HttpContext.Items["Account"] as AccountInfo;
    public ISender Mediator => HttpContext.RequestServices.GetRequiredService<ISender>();
}
