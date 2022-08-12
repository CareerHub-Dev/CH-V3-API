using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApiControllerBase : ControllerBase
{
    public ISender Mediator => HttpContext.RequestServices.GetRequiredService<ISender>();
}
