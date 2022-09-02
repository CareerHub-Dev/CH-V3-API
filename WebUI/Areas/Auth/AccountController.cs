using Application.Accounts.Commands.AccountOwnsToken;
using Application.Accounts.Commands.ChangePassword;
using Application.Accounts.Commands.RevokeToken;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;
using WebUI.Common.Models.Account;

namespace WebUI.Areas.Auth;

[Authorize]
[Route("api/Auth/[controller]")]
public class AccountController : ApiControllerBase
{
    /// <summary>
    /// Auth
    /// </summary>
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

        if (!await Mediator.Send(new AccountOwnsTokenCommand { Token = revokeToken.Token, AccountId = AccountInfo!.Id }))
        {
            return Problem(title: "Not Found", statusCode: StatusCodes.Status404NotFound, detail: "Token is not found");
        }

        await Mediator.Send(new RevokeTokenCommand { Token = revokeToken.Token, IpAddress = IpAddress() });

        return Ok(new { message = "Token revoked" });
    }

    /// <summary>
    /// Auth
    /// </summary>
    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest changePassword)
    {
        await Mediator.Send(new ChangePasswordCommand
        {
            OldPassword = changePassword.OldPassword,
            NewPassword = changePassword.NewPassword,
            AccountId = AccountInfo!.Id,
        });

        return Ok(new { message = "Password change successful" });
    }

    // helper methods
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
