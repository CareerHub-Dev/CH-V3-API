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
    public async Task<IEnumerable<AdminDTO>> GetAdmins([FromQuery] GetAdminsWithPaginationWithSearchWithFilterView view)
    {
        var result = await Mediator.Send(new GetAdminsWithPaginationWithSearchWithFilterQuery
        {
            PageNumber = view.PageNumber,
            PageSize = view.PageSize,
            SearchTerm = view.SearchTerm,
            WithoutAdminId = AccountInfo!.Id,
            IsVerified = view.IsVerified,
            IsSuperAdmin = view.IsSuperAdmin,
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return result;
    }

    /// <remarks>
    /// Admin:
    /// 
    ///     Create admin (sends an e-mail under the hood)
    ///
    /// </remarks>
    [HttpPost("invite")]
    public async Task<ActionResult<Guid>> InviteAdmin(InviteAdminCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{adminId}")]
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
    public async Task<IActionResult> SendInviteAdminEmail(SendInviteAdminEmailCommand command)
    {
        await Mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{adminId}")]
    public async Task<IActionResult> DeleteAdmin(Guid adminId)
    {
        await Mediator.Send(new DeleteAdminCommand(adminId));

        return NoContent();
    }
}
