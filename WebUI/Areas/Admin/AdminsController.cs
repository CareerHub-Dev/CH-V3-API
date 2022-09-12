using Application.Admins.Commands.DeleteAdmin;
using Application.Admins.Commands.InviteAdmin;
using Application.Admins.Commands.UpdateAdmin;
using Application.Admins.Queries;
using Application.Emails.Commands;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebUI.Authorize;
using WebUI.Common.Models.Admin;

namespace WebUI.Areas.Admin;

[Authorize("SuperAdmin")]
[Route("api/Admin/[controller]")]
public class AdminsController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AdminDTO>))]
    public async Task<IActionResult> GetAdmins([FromQuery] GetAdminsWithPaginationWithSearchWithFilterView view)
    {
        var result = await Mediator.Send(new GetAdminsWithPaginationWithSearchWithFilterQuery
        {
            PageNumber = view.PageNumber,
            PageSize = view.PageSize,

            SearchTerm = view.SearchTerm,

            WithoutAdminId = AccountInfo!.Id,
            IsAdminVerified = view.IsAdminVerified,
            IsSuperAdmin = view.IsSuperAdmin,
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("{adminId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AdminDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAdmin(Guid adminId)
    {
        return Ok(await Mediator.Send(new GetAdminQuery(adminId)));
    }

    /// <remarks>
    /// Admin:
    /// 
    ///     Create admin (sends an e-mail under the hood)
    ///
    /// </remarks>
    [HttpPost("invite")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Guid))]
    public async Task<IActionResult> InviteAdmin(InviteAdminCommand command)
    {
        var result = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetAdmin), new { adminId = result }, result);
    }

    [HttpPut("{adminId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAdmin(Guid adminId, UpdateAdminCommand command)
    {
        if (adminId != command.AdminId)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    /// <remarks>
    /// Admin:
    /// 
    ///     sends an e-mail
    ///
    /// </remarks>
    [HttpPost("send-invite-email")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SendInviteAdminEmail(SendInviteAdminEmailCommand command)
    {
        await Mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{adminId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAdmin(Guid adminId)
    {
        await Mediator.Send(new DeleteAdminCommand(adminId));

        return NoContent();
    }
}
