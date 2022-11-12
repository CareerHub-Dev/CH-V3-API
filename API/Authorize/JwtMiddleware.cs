using Application.Accounts.Queries.Identify;
using MediatR;

namespace API.Authorize;

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
            var result = await mediator.Send(new IdentifyQuery { JwtToken = token });

            context.Items["Account"] =
                result != null && result.IsVerified ?
                    new AccountInfo { Id = result.Id, Role = result.Role, IsBanned = result.IsBanned } :
                    null;
        }

        await _next(context);
    }
}
