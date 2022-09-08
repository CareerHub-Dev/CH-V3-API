using Application.Accounts.Commands.ChangePassword;
using Application.Accounts.Commands.DeleteAccount;
using Application.Accounts.Commands.RevokeToken;
using Application.Accounts.Queries.AccountOwnsToken;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;
using WebUI.Common.Models.Account;

namespace WebUI.Areas.Auth;

[Authorize]
[Route("api/Auth/[controller]")]
public class AccountsController : ApiControllerBase
{
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

        if (!await Mediator.Send(new AccountOwnsTokenQuery { Token = view.Token, AccountId = AccountInfo!.Id }))
        {
            return Problem(title: "Token was not found.", statusCode: StatusCodes.Status404NotFound, detail: "Account don't own this token.");
        }

        await Mediator.Send(new RevokeTokenCommand { Token = view.Token });

        return Ok(new { message = "Token revoked" });
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordView view)
    {
        await Mediator.Send(new ChangePasswordCommand
        {
            OldPassword = view.OldPassword,
            NewPassword = view.NewPassword,
            AccountId = AccountInfo!.Id,
        });

        return Ok(new { message = "Password change successful" });
    }

    [HttpDelete("own")]
    public async Task<IActionResult> DeleteAccount()
    {
        await Mediator.Send(new DeleteAccountCommand(AccountInfo!.Id));

        return Ok(new { message = "Account delete successful" });
    }
}
