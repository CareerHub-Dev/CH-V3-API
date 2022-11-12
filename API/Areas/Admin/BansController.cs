using API.Authorize;
using Application.Bans.Commands.CreateBan;
using Application.Bans.Commands.DeleteBan;
using Application.Bans.Commands.UpdateBan;
using Application.Bans.Queries.GetBan;
using Application.Common.DTO.Bans;
using Application.Common.DTO.Tags;
using Application.Common.Enums;
using Microsoft.AspNetCore.Mvc;

namespace API.Areas.Admin;

[Authorize(Role.Admin, Role.SuperAdmin)]
[Route("api/Admin/[controller]")]
public class BansController : ApiControllerBase
{
    [HttpGet("{banId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BanDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBan(Guid banId)
    {
        return Ok(await Mediator.Send(new GetBanQuery(banId)));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    public async Task<IActionResult> CreateBan(CreateBanCommand command)
    {
        var result = await Mediator.Send(command);

        return Ok(result);
    }

    [HttpPut("{banId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateBan(Guid banId, UpdateBanCommand command)
    {
        if (banId != command.BanId)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{banId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteBan(Guid banId)
    {
        await Mediator.Send(new DeleteBanCommand(banId));

        return NoContent();
    }
}
