using API.Authorize;
using Application.Common.DTO.Companies;
using Application.Common.DTO.JobOffers;
using Application.Common.Enums;
using Application.Companies.Commands.VerifiedStudentSubscribeToVerifiedCompany;
using Application.Companies.Commands.VerifiedStudentUnsubscribeFromVerifiedCompany;
using Application.Companies.Queries.GetAmountJobOffersOfCompany;
using Application.Companies.Queries.GetAmountStudentSubscribersOfCompany;
using Application.Companies.Queries.GetDetailedCompany;
using Application.Companies.Queries.GetFollowedShortCompaniesWithStatsForFollowerStudentWithPaginig;
using Application.Companies.Queries.IsVerifiedStudentSubscribedToVerifiedCompany;
using Application.JobOffers.Queries.GetFollowedDetiledJobOffersWithStatsOfCompanyForFollowerStudentWithPaging;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using CompanyStatsFilter = Application.Companies.Queries.Models.StatsFilter;
using JobOfferStatsFilter = Application.JobOffers.Queries.Models.StatsFilter;

namespace API.Areas.Student;

[Authorize(Role.Student)]
[Route("api/Student/[controller]")]
public class CompaniesController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FollowedShortCompanyWithStatsDTO>))]
    public async Task<IActionResult> GetCompanies(
        [FromQuery] string? order,
        [FromQuery] string? search,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Sender.Send(new GetFollowedShortCompaniesWithStatsForFollowerStudentWithPagingQuery
        {
            FollowerStudentId = AccountInfo!.Id,

            PageNumber = pageNumber,
            PageSize = pageSize,

            SearchTerm = search ?? string.Empty,

            IsCompanyMustBeVerified = true,

            StatsFilter = new CompanyStatsFilter
            {
                IsJobOfferMustBeActive = true,

                IsSubscriberMustBeVerified = true,
            },

            OrderByExpression = order ?? "Name",
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("{companyId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DetailedCompanyDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCompany(Guid companyId)
    {
        return Ok(await Sender.Send(new GetDetailedCompanyQuery
        {
            CompanyId = companyId,
            IsCompanyMustBeVerified = true,
        }));
    }

    [HttpGet("{companyId}/amount-subscribers")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountStudentSubscribersOfCompany(Guid companyId)
    {
        return Ok(await Sender.Send(new GetAmountStudentSubscribersOfCompanyQuery
        {
            CompanyId = companyId,
            IsCompanyMustBeVerified = true,

            IsSubscriberMustBeVerified = true,
        }));
    }

    [HttpGet("{companyId}/amount-jobOffers")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountJobOffersOfCompany(Guid companyId)
    {
        return Ok(await Sender.Send(new GetAmountJobOffersOfCompanyQuery
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
        return Ok(await Sender.Send(new IsVerifiedStudentSubscribedToVerifiedCompanyQuery
        {
            StudentId = AccountInfo!.Id,
            CompanyId = companyId
        }));
    }

    [HttpPost("{companyId}/subscribe")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SubscribeToCompany(Guid companyId)
    {
        await Sender.Send(new VerifiedStudentSubscribeToVerifiedCompanyCommand
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
        await Sender.Send(new VerifiedStudentUnsubscribeFromVerifiedCompanyCommand
        {
            StudentId = AccountInfo!.Id,
            CompanyId = companyId
        });

        return NoContent();
    }

    [HttpGet("{companyId}/JobOffers")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FollowedDetiledJobOfferWithStatsDTO>))]
    public async Task<IActionResult> GetJobOffersOfCompany(
        Guid companyId,
        [FromQuery] string? order,
        [FromQuery] string? search,
        [FromQuery] JobType? jobType,
        [FromQuery] WorkFormat? workFormat,
        [FromQuery] ExperienceLevel? experienceLevel,
        [FromQuery] Guid? jobPositionId,
        [FromQuery] List<Guid>? tagIds,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Sender.Send(new GetFollowedDetiledJobOffersWithStatsOfCompanyForFollowerStudentWithPagingQuery
        {
            FollowerStudentId = AccountInfo!.Id,

            CompanyId = companyId,
            IsCompanyOfJobOfferMustBeVerified = true,

            PageNumber = pageNumber,
            PageSize = pageSize,

            SearchTerm = search ?? string.Empty,

            IsJobOfferMustBeActive = true,
            MustHaveWorkFormat = workFormat,
            MustHaveJobType = jobType,
            MustHaveExperienceLevel = experienceLevel,
            MustHaveJobPositionId = jobPositionId,
            MustHaveTagIds = tagIds,

            StatsFilter = new JobOfferStatsFilter
            {
                IsStudentOfAppliedCVMustBeVerified = true,

                IsSubscriberMustBeVerified = true,
            },

            OrderByExpression = order ?? "StartDate",
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }
}
