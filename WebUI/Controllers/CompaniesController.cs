using Application.Companies.Commands.InviteCompany;
using Application.Emails.Commands;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;
using WebUI.DTO.Company;

namespace WebUI.Controllers;

public class CompaniesController : ApiControllerBase
{
    /// <summary>
    /// Admin
    /// </summary>
    /// <remarks>
    /// Admin:
    /// 
    ///     Create company (sends an e-mail under the hood)
    ///
    /// </remarks>
    [HttpPost("invite")]
    [Authorize("Admin")]
    public async Task<ActionResult<Guid>> InviteCompany(InviteCompanyRequest inviteCompany)
    {
        return await Mediator.Send(new InviteCompanyCommand { Email = inviteCompany.Email });
    }

    /// <summary>
    /// Admin
    /// </summary>
    /// <remarks>
    /// Admin:
    /// 
    ///     sends an e-mail
    ///
    /// </remarks>
    [HttpPost("send-invite-email")]
    [Authorize("Admin")]
    public async Task<IActionResult> SendInviteCompanyEmail(SendInviteCompanyEmailRequest sendInviteCompanyEmail)
    {
        await Mediator.Send(new SendInviteCompanyEmailCommand(sendInviteCompanyEmail.CompanyId));
        return Ok();
    }
}
