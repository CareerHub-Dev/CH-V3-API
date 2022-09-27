using Application.Accounts.Commands.ChangeActivationStatus;
using Application.Accounts.Queries.GetBriefAccount;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;

namespace WebUI.Areas.Admin;

[Authorize("Admin", "SuperAdmin")]
[Route("api/Admin/[controller]")]
public class AccountsController : ApiControllerBase
{
    [HttpGet("{accountId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BriefAccountDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBriefAccount(Guid accountId)
    {
        return Ok(await Mediator.Send(new GetBriefAccountQuery(accountId)));
    }

    [HttpPut("{accountId}/change-activation-status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangeActivationStatusOfAccount(Guid accountId, ChangeActivationStatusCommand command)
    {
        if (accountId != command.AccountId)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }
}
