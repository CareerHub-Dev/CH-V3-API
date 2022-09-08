using Application.Accounts.Commands.RevokeToken;
using Application.Accounts.Queries.GetAccountBrief;
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
    public async Task<AccountBriefDTO> GetAccountBrief(Guid accountId)
    {
        var result = await Mediator.Send(new GetAccountBriefQuery(accountId));

        return result;
    }

    /// <remarks>
    /// Admin:
    /// 
    ///     Revoke any Token
    ///
    /// </remarks>
    [HttpPost("revoke-token")]
    public async Task<IActionResult> RevokeTokenAsync(RevokeTokenCommand command)
    {
        await Mediator.Send(command);

        return Ok(new { message = "Token revoked" });
    }
}
