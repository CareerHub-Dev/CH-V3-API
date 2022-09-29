using Application.Companies.Commands.DeleteCompany;
using Application.Companies.Commands.InviteCompany;
using Application.Companies.Commands.UpdateCompanyDetail;
using Application.Companies.Commands.UpdateCompanyBanner;
using Application.Companies.Commands.UpdateCompanyLogo;
using Application.Companies.Queries.GetAmount;
using Application.Companies.Queries.GetCompanies;
using Application.Companies.Queries.GetCompany;
using Application.Emails.Commands;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebUI.Authorize;
using Application.Common.DTO.Companies;
using Application.Common.DTO.CompanyLinks;
using Application.CompanyLinks.Queries.GetCompanyLinks;
using Domain.Enums;

namespace WebUI.Areas.Admin;

[Authorize("Admin", "SuperAdmin")]
[Route("api/Admin/[controller]")]
public class CompaniesController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CompanyWithStatsDTO>))]
    public async Task<IActionResult> GetCompanies(
        [FromQuery] ActivationStatus? activationStatus,
        [FromQuery] bool? isCompanyMustBeVerified,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string orderByExpression = "Name",
        [FromQuery] string searchTerm = "")
    {
        var result = await Mediator.Send(new GetCompaniesWithStatsWithPaginationWithSearchWithFilterQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm,
            IsCompanyMustBeVerified = isCompanyMustBeVerified,
            ActivationStatus = activationStatus,
            OrderByExpression = orderByExpression
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("{companyId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CompanyDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCompany(Guid companyId)
    {
        return Ok(await Mediator.Send(new GetCompanyWithFilterQuery
        {
            CompanyId = companyId
        }));
    }

    [HttpGet("{companyId}/amount-subscribers")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountSubscribersOfCompany(Guid companyId)
    {
        return Ok(await Mediator.Send(new GetAmountSubscribersOfCompanyWithFilterQuery
        {
            CompanyId = companyId
        }));
    }

    [HttpGet("{companyId}/amount-jobOffers")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountJobOffersOfCompany(Guid companyId)
    {
        return Ok(await Mediator.Send(new GetAmountJobOffersOfCompanyWithFilterQuery
        {
            CompanyId = companyId
        }));
    }

    /// <remarks>
    /// Admin:
    /// 
    ///     Create company (sends an e-mail under the hood)
    ///
    /// </remarks>
    [HttpPost("invite")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    public async Task<IActionResult> InviteCompany(InviteCompanyCommand command)
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
    public async Task<IActionResult> SendInviteCompanyEmail(SendInviteCompanyEmailCommand command)
    {
        await Mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{companyId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCompany(Guid companyId)
    {
        await Mediator.Send(new DeleteCompanyCommand(companyId));

        return NoContent();
    }

    [HttpPut("{companyId}/detail")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateCompanyDetail(Guid companyId, UpdateCompanyDetailCommand command)
    {
        if (companyId != command.CompanyId)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPost("{companyId}/logo")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid?))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCompanyLogo(Guid companyId, IFormFile? file)
    {
        var result = await Mediator.Send(new UpdateCompanyLogoCommand { CompanyId = companyId, Logo = file });

        return Ok(result);
    }

    [HttpPost("{companyId}/banner")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid?))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCompanyBanner(Guid companyId, IFormFile? file)
    {
        var result = await Mediator.Send(new UpdateCompanyBannerCommand { CompanyId = companyId, Banner = file });

        return Ok(result);
    }

    [HttpGet("{companyId}/CompanyLinks")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CompanyLinkDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCompanyLinksOfCompany(Guid companyId)
    {
        return Ok(await Mediator.Send(new GetCompanyLinksOfCompanyWithFilterQuery
        {
            CompanyId = companyId
        }));
    }
}
