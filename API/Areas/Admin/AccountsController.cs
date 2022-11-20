using API.Authorize;
using Application.Accounts.Queries.GetBriefAccount;
using Application.Bans.Queries.GetBansOfAccount;
using Application.Common.DTO.Accounts;
using Application.Common.DTO.Bans;
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
        return Ok(await Sender.Send(new GetBriefAccountQuery(accountId)));
    }

    [HttpGet("{accountId}/Bans")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BanDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBansOfAccount(
        Guid accountId,
        [FromQuery] string? orderByExpression,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        return Ok(await Sender.Send(new GetBansOfAccountWithPagingQuery
        {
            AccountId = accountId,
            PageNumber = pageNumber,
            PageSize = pageSize,
            OrderByExpression = orderByExpression ?? "Expires"
        }));
    }
}
