using Application.Accounts.Commands.RevokeRefreshToken;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;

namespace WebUI.Areas.Admin;

[Authorize("Admin", "SuperAdmin")]
[Route("api/Admin/[controller]")]
public class RefreshTokensController : ApiControllerBase
{
    /// <remarks>
    /// Admin:
    /// 
    ///     Revoke any Token
    ///
    /// </remarks>
    [HttpPost("revoke-token")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RevokeTokenAsync(RevokeRefreshTokenCommand command)
    {
        await Mediator.Send(command);

        return NoContent();
    }
}
