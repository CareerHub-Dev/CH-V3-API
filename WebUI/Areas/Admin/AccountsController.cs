using Application.Accounts.Commands.RevokeToken;
using Application.Accounts.Query;
using Application.Accounts.Query.Models;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;
using WebUI.Common.Models.Account;

namespace WebUI.Areas.Admin;

[Authorize("Admin", "SuperAdmin")]
[Route("api/Admin/[controller]")]
public class AccountsController : ApiControllerBase
{
    /// <remarks>
    /// Admin:
    /// 
    ///     Get brief info about account (should be used in the admin panel to see who edited or created a record)
    ///
    /// </remarks>
    [HttpGet("{accountId}")]
    public async Task<AccountBriefDTO> GetAccountBrief(Guid accountId)
    {
        var result = await Mediator.Send(new GetAccountBriefQuery(accountId));

        return result;
    }

    /// <remarks>
    /// Admin:
    /// 
    ///     Revoke any Token
    ///
    /// </remarks>
    [HttpPost("revoke-token")]
    public async Task<IActionResult> RevokeTokenAsync(RevokeTokenView view)
    {
        if (string.IsNullOrWhiteSpace(view.Token))
        {
            view.Token = Request.Cookies["refreshToken"];
        }

        if (string.IsNullOrWhiteSpace(view.Token))
        {
            return Problem(title: "Token is required.", statusCode: StatusCodes.Status400BadRequest, detail: "Body or cookies don't contain token.");
        }

        await Mediator.Send(new RevokeTokenCommand { Token = view.Token, IpAddress = IpAddress() });

        return Ok(new { message = "Token revoked" });
    }

    private string IpAddress()
    {
        if (Request.Headers.ContainsKey("X-Forwarded-For"))
        {
            return Request.Headers["X-Forwarded-For"];
        }
        else
        {
            return HttpContext.Connection.RemoteIpAddress!.MapToIPv4().ToString();
        }
    }
}
