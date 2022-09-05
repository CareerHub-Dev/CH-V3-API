using Application.Companies.Commands.DeleteCompany;
using Application.Companies.Commands.InviteCompany;
using Application.Companies.Commands.UpdateCompany;
using Application.Companies.Commands.UpdateCompanyBanner;
using Application.Companies.Commands.UpdateCompanyLogo;
using Application.Companies.Query;
using Application.Companies.Query.Models;
using Application.CompanyLinks.Query;
using Application.Emails.Commands;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebUI.Authorize;
using WebUI.Common.Extentions;
using WebUI.Common.Models.Company;
using WebUI.Common.Models.CompanyLink;

namespace WebUI.Areas.Admin;

[Authorize("Admin", "SuperAdmin")]
[Route("api/Admin/[controller]")]
public class CompaniesController : ApiControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<CompanyWithAmountStatisticDTO>> GetCompanies(
        [FromQuery] GetCompaniesWithAmountStatisticWithPaginationWithSearchWithFilterView view)
    {
        var result = await Mediator.Send(new GetCompaniesWithAmountStatisticWithPaginationWithSearchWithFilterQuery
        {
            PageNumber = view.PageNumber,
            PageSize = view.PageSize,
            SearchTerm = view.SearchTerm,
            IsVerified = view.IsVerified,
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return result;
    }

    /// <remarks>
    /// Admin:
    /// 
    ///     Create company (sends an e-mail under the hood)
    ///
    /// </remarks>
    [HttpPost("invite")]
    public async Task<ActionResult<Guid>> InviteCompany(InviteCompanyCommand command)
    {
        return await Mediator.Send(command);
    }

    /// <remarks>
    /// Admin:
    /// 
    ///     sends an e-mail
    ///
    /// </remarks>
    [HttpPost("send-invite-email")]
    public async Task<IActionResult> SendInviteCompanyEmail(SendInviteCompanyEmailCommand command)
    {
        await Mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{companyId}")]
    public async Task<IActionResult> DeleteCompany(Guid companyId)
    {
        await Mediator.Send(new DeleteCompanyCommand(companyId));

        return NoContent();
    }

    /// <remarks>   
    /// Company
    /// 
    ///     get all CompanyLinks Of Company
    ///
    /// </remarks>
    [HttpGet("{companyId}/companyLinks")]
    public async Task<IEnumerable<CompanyLinkResponse>> GetCompanyLinksOfCompany(Guid companyId)
    {
        var result = await Mediator.Send(new GetCompanyLinksOfCompanyWithFilterQuery
        {
            CompanyId = companyId
        });

        return result.Select(x => new CompanyLinkResponse(x));
    }
}
