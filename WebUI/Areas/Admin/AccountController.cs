using Application.Accounts.Commands.RevokeToken;
using Application.Accounts.Query;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;
using WebUI.Common.Models.Account;

namespace WebUI.Areas.Admin;

[Authorize("Admin")]
[Route("api/Admin/[controller]")]
public class AccountController : ApiControllerBase
{
    /// <remarks>
    /// Admin:
    /// 
    ///     Get brief info about account (should be used in the admin panel to see who edited or created a record)
    ///
    /// </remarks>
    [HttpGet("{accountId}")]
    public async Task<AccountBriefResponse> GetAccountBrief(Guid accountId)
    {
        var result = await Mediator.Send(new GetAccountBriefQuery(accountId));

        return new AccountBriefResponse(result);
    }

    /// <remarks>
    /// Admin:
    /// 
    ///     Revoke any Token
    ///
    /// </remarks>
    [HttpPost("revoke-token")]
    public async Task<IActionResult> RevokeTokenAsync(RevokeTokenRequest revokeToken)
    {
        if (string.IsNullOrWhiteSpace(revokeToken.Token))
        {
            revokeToken.Token = Request.Cookies["refreshToken"];
        }

        if (string.IsNullOrWhiteSpace(revokeToken.Token))
        {
            return Problem(title: "Bad Request", statusCode: StatusCodes.Status400BadRequest, detail: "Token is required");
        }

        await Mediator.Send(new RevokeTokenCommand { Token = revokeToken.Token, IpAddress = IpAddress() });

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
