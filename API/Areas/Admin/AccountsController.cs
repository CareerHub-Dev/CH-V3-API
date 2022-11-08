using Application.Accounts.Queries.GetBriefAccount;
using Microsoft.AspNetCore.Mvc;
using API.Authorize;

namespace API.Areas.Admin;

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
}
