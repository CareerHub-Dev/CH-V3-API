using Application.Companies.Commands.InviteCompany;
using Application.Emails.Commands;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;
using WebUI.DTO.Company;

namespace WebUI.Controllers
{
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
        public async Task<ActionResult<Guid>> InviteCompany(InviteCompanyRequest model)
        {
            return await Mediator.Send(new InviteCompanyCommand { Email = model.Email });
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
        [HttpPost("inviteEmail")]
        [Authorize("Admin")]
        public async Task<IActionResult> SendInviteCompanyEmail(InviteCompanyEmailRequest model)
        {
            await Mediator.Send(new SendInviteCompanyEmailCommand(model.CompanyId));
            return Ok();
        }
    }
}
