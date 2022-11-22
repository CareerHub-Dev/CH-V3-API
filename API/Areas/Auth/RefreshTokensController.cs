using API.Authorize;
using API.DTO.Requests.RefreshTokens;
using Application.RefreshTokens.Commands.RevokeRefreshTokenOfAccount;
using Microsoft.AspNetCore.Mvc;

namespace API.Areas.Auth;

[Authorize]
[Route("api/Auth/[controller]")]
public class RefreshTokensController : ApiControllerBase
{
    [HttpPost("revoke-token")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RevokeTokenAsync(RevokeOwnRefreshTokenRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Token))
        {
            request.Token = Request.Cookies["refreshToken"] ?? "";
        }

        await Sender.Send(new RevokeRefreshTokenOfAccountCommand
        {
            Token = request.Token,
            AccountId = AccountInfo!.Id
        });

        return NoContent();
    }
}
