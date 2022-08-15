using Application.Companies.Commands.InviteCompany;
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
    }
}
