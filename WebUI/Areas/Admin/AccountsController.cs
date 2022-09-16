using Application.Accounts.Commands.RevokeRefreshToken;
using Application.Accounts.Queries.GetBriefAccount;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;

namespace WebUI.Areas.Admin;

[Authorize("Admin", "SuperAdmin")]
[Route("api/Admin/[controller]")]
public class AccountsController : ApiControllerBase
{
    /// <remarks>
    /// Admin:
    /// 
    ///     Get brief info about account (should be used in the admin panel to see who edited or created a record)
    ///
    /// </remarks>
    [HttpGet("{accountId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BriefAccountDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBriefAccount(Guid accountId)
    {
        return Ok(await Mediator.Send(new GetBriefAccountQuery(accountId)));
    }

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
