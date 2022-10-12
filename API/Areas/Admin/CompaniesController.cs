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
using API.Authorize;
using Application.Common.DTO.Companies;
using Application.Common.DTO.CompanyLinks;
using Application.CompanyLinks.Queries.GetCompanyLinks;
using Domain.Enums;
using Application.Common.DTO.JobOffers;
using Application.JobOffers.Queries.GetJobOffers;

namespace API.Areas.Admin;

[Authorize("Admin", "SuperAdmin")]
[Route("api/Admin/[controller]")]
public class CompaniesController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CompanyWithStatsDTO>))]
    public async Task<IActionResult> GetCompanies(
        [FromQuery] ActivationStatus? companyMustHaveActivationStatus,
        [FromQuery] bool? isCompanyMustBeVerified,
        [FromQuery] string? orderByExpression,
        [FromQuery] string? searchTerm,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Mediator.Send(new GetCompaniesWithStatsWithPaginationWithSearchWithFilterWithSortQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm ?? string.Empty,
            IsCompanyMustBeVerified = isCompanyMustBeVerified,
            CompanyMustHaveActivationStatus = companyMustHaveActivationStatus,
            OrderByExpression = orderByExpression ?? "Name"
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
    public async Task<IActionResult> GetAmountStudentSubscribersOfCompany(Guid companyId)
    {
        return Ok(await Mediator.Send(new GetAmountStudentSubscribersOfCompanyWithFilterQuery
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

    [HttpGet("{companyId}/JobOffers")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<DetiledJobOfferWithStatsDTO>))]
    public async Task<IActionResult> GetJobOffersOfCompany(
        Guid companyId,
        [FromQuery] bool? isJobOfferMustBeActive,
        [FromQuery] string? orderByExpression,
        [FromQuery] string? searchTerm,
        [FromQuery] JobType? mustHaveJobType,
        [FromQuery] WorkFormat? mustHaveWorkFormat,
        [FromQuery] ExperienceLevel? mustHaveExperienceLevel,
        [FromQuery] Guid? mustHavejobPositionId,
        [FromQuery] List<Guid>? mustHaveTagIds,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Mediator.Send(new GetDetiledJobOffersWithStatsOfCompanyWithPaginationWithSearchWithFilterWithSortQuery
        {
            CompanyId = companyId,

            PageNumber = pageNumber,
            PageSize = pageSize,

            SearchTerm = searchTerm ?? string.Empty,

            IsJobOfferMustBeActive = isJobOfferMustBeActive,
            MustHaveWorkFormat = mustHaveWorkFormat,
            MustHaveJobType = mustHaveJobType,
            MustHaveExperienceLevel = mustHaveExperienceLevel,
            MustHaveJobPositionId = mustHavejobPositionId,
            MustHaveTagIds = mustHaveTagIds,

            OrderByExpression = orderByExpression ?? "StartDate",
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }
}
