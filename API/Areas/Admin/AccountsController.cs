using API.Authorize;
using Application.Accounts.Queries.GetBriefAccount;
using Application.Common.Enums;
using Microsoft.AspNetCore.Mvc;

namespace API.Areas.Admin;

[Authorize(Role.Admin, Role.SuperAdmin)]
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
