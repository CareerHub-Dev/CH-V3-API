using Application.Common.Interfaces;

namespace WebUI.Authorize;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IAccountManager accountManager, IJwtService jwtService)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
        {
            var accountId = await jwtService.ValidateJwtTokenAsync(token);

            if (accountId.HasValue)
            {
                var accountInfo = await accountManager.GetAccountInfoAsync(accountId.Value);

                context.Items["Account"] = accountInfo is not null ? accountInfo : null; 
            }
        }

        await _next(context);
    }
}
