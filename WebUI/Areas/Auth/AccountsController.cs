using Application.Accounts.Commands.ChangePassword;
using Application.Accounts.Commands.DeleteAccount;
using Application.Accounts.Commands.RevokeRefreshTokenOfAccount;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;
using WebUI.Common.Models.Account;

namespace WebUI.Areas.Auth;

[Authorize]
[Route("api/Auth/[controller]")]
public class AccountsController : ApiControllerBase
{
    [HttpPost("revoke-token")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RevokeTokenAsync(RevokeTokenView view)
    {
        if (string.IsNullOrWhiteSpace(view.Token))
        {
            view.Token = Request.Cookies["refreshToken"] ?? "";
        }

        await Mediator.Send(new RevokeRefreshTokenOfAccountCommand { Token = view.Token, AccountId = AccountInfo!.Id });

        return Ok();
    }

    [Authorize]
    [HttpPost("change-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ChangePassword(ChangePasswordView view)
    {
        await Mediator.Send(new ChangePasswordCommand
        {
            OldPassword = view.OldPassword,
            NewPassword = view.NewPassword,
            AccountId = AccountInfo!.Id,
        });

        return Ok();
    }

    [HttpDelete("own")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteAccount()
    {
        await Mediator.Send(new DeleteAccountCommand(AccountInfo!.Id));

        return Ok();
    }
}
