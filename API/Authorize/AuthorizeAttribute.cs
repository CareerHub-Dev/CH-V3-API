using Application.Common.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Authorize;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly IList<Role> _roles;

    public AuthorizeAttribute(params Role[] roles)
    {
        _roles = roles;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var account = context.HttpContext.Items["Account"] as AccountInfo;

        if (account == null)
        {
            context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
        }
        else if (account.IsBanned)
        {
            context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
        }
        else if (_roles.Any() && !_roles.Contains(account.Role))
        {
            context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
        }
    }
}
