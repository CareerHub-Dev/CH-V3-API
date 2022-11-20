using API.Authorize;
using API.DTO.Requests.Accounts;
using Application.Accounts.Commands.ChangePassword;
using Microsoft.AspNetCore.Mvc;

namespace API.Areas.Auth;

[Authorize]
[Route("api/Auth/[controller]")]
public class AccountController : ApiControllerBase
{
    [HttpPost("change-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest view)
    {
        await Sender.Send(new ChangePasswordCommand
        {
            OldPassword = view.OldPassword,
            NewPassword = view.NewPassword,
            AccountId = AccountInfo!.Id,
        });

        return NoContent();
    }
}
