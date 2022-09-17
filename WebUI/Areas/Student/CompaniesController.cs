using Application.Companies.Commands.VerifiedStudentSubscribeToVerifiedCompany;
using Application.Companies.Commands.VerifiedStudentUnsubscribeFromVerifiedCompany;
using Application.Companies.Queries;
using Application.Companies.Queries.GetAmount;
using Application.Companies.Queries.GetCompanies;
using Application.Companies.Queries.GetCompany;
using Application.Companies.Queries.Models;
using Application.CompanyLinks.Queries;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebUI.Authorize;

namespace WebUI.Areas.Student;

[Authorize("Student")]
[Route("api/Student/[controller]")]
public class CompaniesController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CompanyWithStatsDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCompanies(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] bool? isCompanyMustBeVerified = null)
    {
        var result = await Mediator.Send(new GetFollowedDetailedCompaniesWithStatsForFollowerStudentWithPaginationWithSearchWithFilterQuery
        {
            FollowerStudentId = AccountInfo!.Id,
            IsFollowerStudentMustBeVerified = true,
            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm,
            IsCompanyMustBeVerified = isCompanyMustBeVerified,
            StatsFilter = new StatsFilter
            {
                IsJobOfferMustBeActive = true,
                IsSubscriberMustBeVerified = true
            }
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("{companyId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FollowedDetailedCompanyDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCompany(Guid companyId)
    {
        return Ok(await Mediator.Send(new GetFollowedDetailedCompanyForFollowerStudentWithFilterQuery
        {
            FollowerStudentId = AccountInfo!.Id,
            IsFollowerStudentMustBeVerified = true,
            CompanyId = companyId,
            IsCompanyMustBeVerified = true
        }));
    }

    [HttpGet("{companyId}/amount-subscribers")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountSubscribersOfOwnCompany(Guid companyId)
    {
        return Ok(await Mediator.Send(new GetAmountSubscribersOfCompanyWithFilterQuery
        {
            CompanyId = companyId,
            IsCompanyMustBeVerified = true,
            IsSubscriberMustBeVerified = true
        }));
    }

    [HttpGet("{companyId}/CompanyLinks")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CompanyLinkDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCompanyLinksOfCompany(Guid companyId)
    {
        return Ok(await Mediator.Send(new GetCompanyLinksOfCompanyWithFilterQuery
        {
            CompanyId = companyId,
            IsCompanyMustBeVerified = true,
        }));
    }

    [HttpGet("{companyId}/amount-jobOffers")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountJobOffersOfOwnCompany(Guid companyId)
    {
        return Ok(await Mediator.Send(new GetAmountJobOffersOfCompanyWithFilterQuery
        {
            CompanyId = companyId,
            IsCompanyMustBeVerified = true,
            IsJobOfferMustBeActive = true
        }));
    }

    [HttpGet("{companyId}/subscribe")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> IsStudentSubscribedToCompany(Guid companyId)
    {
        await Mediator.Send(new IsVerifiedStudentSubscribedToVerifiedCompanyQuery
        {
            StudentId = AccountInfo!.Id,
            CompanyId = companyId
        });

        return NoContent();
    }

    [HttpPost("{companyId}/subscribe")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SubscribeToCompany(Guid companyId)
    {
        await Mediator.Send(new VerifiedStudentSubscribeToVerifiedCompanyCommand
        {
            StudentId = AccountInfo!.Id,
            CompanyId = companyId
        });

        return NoContent();
    }

    [HttpDelete("{companyId}/subscribe")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UnsubscribeFromCompany(Guid companyId)
    {
        await Mediator.Send(new VerifiedStudentUnsubscribeFromVerifiedCompanyCommand
        {
            StudentId = AccountInfo!.Id,
            CompanyId = companyId
        });

        return NoContent();
    }
}
