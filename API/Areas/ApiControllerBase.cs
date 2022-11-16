using API.Authorize;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Areas;

[ApiController]
public class ApiControllerBase : ControllerBase
{
    public AccountInfo? AccountInfo => HttpContext.Items["Account"] as AccountInfo;
    public ISender Sender => HttpContext.RequestServices.GetRequiredService<ISender>();
}
