using Application.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebUI.Authorize;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly IList<string> _roles;

    public AuthorizeAttribute(params string[] roles)
    {
        _roles = roles;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var account = context.HttpContext.Items["Account"] as AccountInfo;

        if (account == null)
        {
            context.Result = new UnauthorizedResult();
        }
        else if (_roles.Any() && !_roles.Contains(account.Role))
        {
            context.Result = new ForbidResult();
        }
    }
}
