using Application.Companies.Commands.DeleteCompany;
using Application.Companies.Commands.InviteCompany;
using Application.Companies.Commands.UpdateCompany;
using Application.Companies.Commands.UpdateCompanyBanner;
using Application.Companies.Commands.UpdateCompanyLogo;
using Application.Companies.Queries;
using Application.Companies.Queries.Models;
using Application.CompanyLinks.Queries;
using Application.Emails.Commands;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebUI.Authorize;
using WebUI.Common.Models.Company;

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

    [HttpGet("{companyId}")]
    public async Task<CompanyDTO> GetCompany(Guid companyId)
    {
        return await Mediator.Send(new GetCompanyWithFilterQuery
        {
            CompanyId = companyId
        });
    }

    [HttpGet("{companyId}/companyLinks")]
    public async Task<IEnumerable<CompanyLinkDTO>> GetCompanyLinksOfCompany(Guid companyId)
    {
        return await Mediator.Send(new GetCompanyLinksOfCompanyWithFilterQuery
        {
            CompanyId = companyId
        });
    }

    [HttpGet("{companyId}/amountSubscribers")]
    public async Task<int> GetAmountSubscribersOfCompany(Guid companyId)
    {
        return await Mediator.Send(new GetAmountSubscribersOfCompanyWithFilterQuery
        {
            CompanyId = companyId
        });
    }

    [HttpGet("{companyId}/amountJobOffers")]
    public async Task<int> GetAmountJobOffersOfCompany(Guid companyId)
    {
        return await Mediator.Send(new GetAmountJobOffersOfCompanyWithFilterQuery
        {
            CompanyId = companyId
        });
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

    [HttpPut("{companyId}")]
    public async Task<IActionResult> UpdateCompany(Guid companyId, UpdateCompanyCommand command)
    {
        if (companyId != command.CompanyId)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPost("{companyId}/logo")]
    public async Task<Guid?> UpdateCompanyLogo(Guid companyId, IFormFile? file)
    {
        return await Mediator.Send(new UpdateCompanyBannerCommand { CompanyId = companyId, Banner = file });
    }

    [HttpPost("{companyId}/banner")]
    public async Task<Guid?> UpdateCompanyBanner(Guid companyId, IFormFile? file)
    {
        return await Mediator.Send(new UpdateCompanyLogoCommand { CompanyId = companyId, Logo = file });
    }
}
