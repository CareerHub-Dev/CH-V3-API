﻿using Application.Admins.Commands.DeleteAdmin;
using Application.Admins.Commands.InviteAdmin;
using Application.Admins.Queries;
using Application.Common.Models.Filtration.Admin;
using Application.Common.Models.Pagination;
using Application.Emails.Commands;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebUI.Authorize;
using WebUI.DTO.Admin;

namespace WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Admin")]
    public class AdminsController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminResponse>>> GetAdmins([FromQuery] PaginationParameters paginationParameters, [FromQuery] AdminListFilter filter)
        {
            var result = await Mediator.Send(new GetAdminsQuery
            {
                PaginationParameters = paginationParameters,
                FilterParameters = new AdminListFilterParameters { WithoutAdminId = AccountInfo!.Id, IsVerified = filter.IsVerified }
            });

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

            return Ok(result.Select(x => new AdminResponse(x)));
        }

        [HttpGet("{adminId}")]
        public async Task<ActionResult<AdminResponse>> GetAdmin(Guid adminId)
        {
            var result = await Mediator.Send(new GetAdminQuery
            {
                AdminId = adminId
            });

            return new AdminResponse(result);
        }

        /// <remarks>
        /// Admin:
        /// 
        ///     Create admin (sends an e-mail under the hood)
        ///
        /// </remarks>
        [HttpPost("invite")]
        public async Task<ActionResult<Guid>> InviteAdmin(InviteAdminRequest model)
        {
            return await Mediator.Send(new InviteAdminCommand { Email = model.Email });
        }

        /// <remarks>
        /// Admin:
        /// 
        ///     sends an e-mail
        ///
        /// </remarks>
        [HttpPost("send-invite-email")]
        public async Task<IActionResult> SendInviteAdminEmail(SendInviteAdminEmailRequest model)
        {
            await Mediator.Send(new SendInviteAdminEmailCommand(model.AdminId));
            return Ok();
        }

        [HttpDelete("{adminId}")]
        public async Task<IActionResult> DeleteAdmin(Guid adminId)
        {
            await Mediator.Send(new DeleteAdminCommand(adminId));

            return NoContent();
        }
    }
}