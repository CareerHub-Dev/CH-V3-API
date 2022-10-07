using MediatR;
using Microsoft.AspNetCore.Mvc;
using API.Authorize;

namespace API.Areas;

[ApiController]
public class ApiControllerBase : ControllerBase
{
    public AccountInfo? AccountInfo => HttpContext.Items["Account"] as AccountInfo;
    public ISender Mediator => HttpContext.RequestServices.GetRequiredService<ISender>();
}
