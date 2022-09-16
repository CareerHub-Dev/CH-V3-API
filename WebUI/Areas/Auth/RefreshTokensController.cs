using Application.RefreshTokens.Commands.RevokeRefreshTokenOfAccount;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;
using WebUI.Common.Models.Account;

namespace WebUI.Areas.Auth;

[Authorize("Student")]
[Route("api/Student/[controller]")]
public class RefreshTokensController : ApiControllerBase
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
}
