using Application.Accounts.Commands.ChangePassword;
using Application.Accounts.Commands.DeleteAccount;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;
using WebUI.Common.Models.Account;

namespace WebUI.Areas.Auth;

[Authorize]
[Route("api/Auth/[controller]")]
public class AccountController : ApiControllerBase
{
    [HttpPost("change-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangePassword(ChangePasswordView view)
    {
        await Mediator.Send(new ChangePasswordCommand
        {
            OldPassword = view.OldPassword,
            NewPassword = view.NewPassword,
            AccountId = AccountInfo!.Id,
        });

        return NoContent();
    }

    [HttpDelete("delete-account")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAccount()
    {
        await Mediator.Send(new DeleteAccountCommand(AccountInfo!.Id));

        return NoContent();
    }
}
