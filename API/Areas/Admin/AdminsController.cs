using API.Authorize;
using Application.Admins.Commands.DeleteAdmin;
using Application.Admins.Commands.InviteAdmin;
using Application.Admins.Queries.GetAdmin;
using Application.Admins.Queries.GetAdmins;
using Application.Common.DTO.Admins;
using Application.Common.Enums;
using Application.Emails.Commands.SendInviteAdminEmail;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Areas.Admin;

[Authorize(Role.SuperAdmin)]
[Route("api/Admin/[controller]")]
public class AdminsController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AdminDTO>))]
    public async Task<IActionResult> GetAdmins(
        [FromQuery] bool? isAdminMustBeVerified,
        [FromQuery] bool? isSuperAdmin,
        [FromQuery] string? orderByExpression,
        [FromQuery] string? searchTerm,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Sender.Send(new GetAdminsWithPaginationWithSearchWithFilterWithSortQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,

            SearchTerm = searchTerm ?? string.Empty,

            WithoutAdminId = AccountInfo!.Id,
            IsAdminMustBeVerified = isAdminMustBeVerified,
            IsSuperAdmin = isSuperAdmin,

            OrderByExpression = orderByExpression ?? "Email"
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("{adminId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AdminDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAdmin(Guid adminId)
    {
        return Ok(await Sender.Send(new GetAdminQuery(adminId)));
    }

    /// <remarks>
    /// Admin:
    /// 
    ///     Create admin (sends an e-mail under the hood)
    ///
    /// </remarks>
    [HttpPost("invite")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    public async Task<IActionResult> InviteAdmin(InviteAdminCommand command)
    {
        var result = await Sender.Send(command);

        return Ok(result);
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
        await Sender.Send(command);

        return NoContent();
    }

    [HttpDelete("{adminId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAdmin(Guid adminId)
    {
        await Sender.Send(new DeleteAdminCommand(adminId));

        return NoContent();
    }
}
