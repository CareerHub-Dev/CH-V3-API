﻿using Application.Accounts.Commands.ChangePassword;
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
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RevokeTokenAsync(RevokeTokenView view)
    {
        if (string.IsNullOrWhiteSpace(view.Token))
        {
            view.Token = Request.Cookies["refreshToken"] ?? "";
        }

        await Mediator.Send(new RevokeRefreshTokenOfAccountCommand { Token = view.Token, AccountId = AccountInfo!.Id });

        return NoContent();
    }

    [Authorize]
    [HttpPost("change-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
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

        return Ok();
    }

    [HttpDelete("own")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAccount()
    {
        await Mediator.Send(new DeleteAccountCommand(AccountInfo!.Id));

        return NoContent();
    }
}
