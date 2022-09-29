﻿using Application.Admins.Commands.DeleteAdmin;
using Application.Admins.Commands.InviteAdmin;
using Application.Admins.Queries.GetAdmin;
using Application.Admins.Queries.GetAdmins;
using Application.Common.DTO.Admins;
using Application.Emails.Commands;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebUI.Authorize;

namespace WebUI.Areas.Admin;

[Authorize("SuperAdmin")]
[Route("api/Admin/[controller]")]
public class AdminsController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AdminDTO>))]
    public async Task<IActionResult> GetAdmins(
        [FromQuery] bool? isAdminMustBeVerified,
        [FromQuery] bool? isSuperAdmin,
        [FromQuery] ActivationStatus? activationStatus,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string orderByExpression = "Email",
        [FromQuery] string searchTerm = "")
    {
        var result = await Mediator.Send(new GetAdminsWithPaginationWithSearchWithFilterWithSortQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,

            SearchTerm = searchTerm,

            WithoutAdminId = AccountInfo!.Id,
            IsAdminMustBeVerified = isAdminMustBeVerified,
            IsSuperAdmin = isSuperAdmin,
            ActivationStatus = activationStatus,

            OrderByExpression = orderByExpression
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    public async Task<IActionResult> InviteAdmin(InviteAdminCommand command)
    {
        var result = await Mediator.Send(command);

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
