using Application.Auth.Commands.Identify;
using MediatR;

namespace WebUI.Authorize;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, ISender mediator)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
        {
            var result = await mediator.Send(new IdentifyCommand { JwtToken = token });

            context.Items["Account"] = result != null ? new AccountInfo { Id = result.Id, Role = result.Role } : null;
        }

        await _next(context);
    }
}
