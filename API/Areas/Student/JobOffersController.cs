using API.Authorize;
using Application.Common.DTO.JobOffers;
using Application.JobOffers.Commands.VerifiedActiveStudentSubscribeToActiveJobOfferWithVerifiedActiveCompany;
using Application.JobOffers.Commands.VerifiedStudentUnubscribeFromActiveJobOffer;
using Application.JobOffers.Queries;
using Application.JobOffers.Queries.GetAmount;
using Application.JobOffers.Queries.GetJobOffer;
using Application.JobOffers.Queries.GetJobOffers;
using Application.JobOffers.Queries.Models;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Areas.Student;

[Authorize("Student")]
[Route("api/Student/[controller]")]
public class JobOffersController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FollowedDetiledJobOfferWithStatsDTO>))]
    public async Task<IActionResult> GetJobOffers(
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
        var result = await Mediator.Send(new GetFollowedDetiledJobOffersWithStatsForFollowerStudentWithPaginationWithSearchWithFilterWithSortQuery
        {
            FollowerStudentId = AccountInfo!.Id,

            PageNumber = pageNumber,
            PageSize = pageSize,

            SearchTerm = searchTerm ?? string.Empty,

            IsJobOfferMustBeActive = true,
            MustHaveWorkFormat = mustHaveWorkFormat,
            MustHaveJobType = mustHaveJobType,
            MustHaveExperienceLevel = mustHaveExperienceLevel,
            MustHaveJobPositionId = mustHavejobPositionId,
            MustHaveTagIds = mustHaveTagIds,
            IsCompanyOfJobOfferMustBeVerified = true,
            CompanyOfJobOfferMustHaveActivationStatus = ActivationStatus.Active,

            StatsFilter = new StatsFilter
            {
                IsStudentOfAppliedCVMustBeVerified = true,
                StudentOfCVMustHaveActivationStatus = ActivationStatus.Active,

                IsSubscriberMustBeVerified = true,
                SubscriberMustHaveActivationStatus = ActivationStatus.Active
            },

            OrderByExpression = orderByExpression ?? "StartDate",
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("{jobOfferId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JobOfferDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetJobOffer(Guid jobOfferId)
    {
        return Ok(await Mediator.Send(new GetJobOfferWithFilterQuery
        {
            JobOfferId = jobOfferId,
            IsJobOfferMustBeActive = true,
            IsCompanyOfJobOfferMustBeVerified = true,
            CompanyOfJobOfferMustHaveActivationStatus = ActivationStatus.Active,
        }));
    }

    [HttpGet("{jobOfferId}/amount-student-subscribers")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountStudentSubscribersOfJobOffer(Guid jobOfferId)
    {
        return Ok(await Mediator.Send(new GetAmountStudentSubscribersOfJobOfferWithFilterQuery
        {
            JobOfferId = jobOfferId,
            IsJobOfferMustBeActive = true,
            IsCompanyOfJobOfferMustBeVerified = true,
            CompanyOfJobOfferMustHaveActivationStatus = ActivationStatus.Active,

            IsSubscriberMustBeVerified = true,
            SubscriberMustHaveActivationStatus = ActivationStatus.Active
        }));
    }

    [HttpGet("{jobOfferId}/amount-applied-cvs")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountAppliedCVsOfJobOffer(Guid jobOfferId)
    {
        return Ok(await Mediator.Send(new GetAmountAppliedCVsOfJobOfferWithFilterQuery
        {
            JobOfferId = jobOfferId,
            IsJobOfferMustBeActive = true,
            IsCompanyOfJobOfferMustBeVerified = true,
            CompanyOfJobOfferMustHaveActivationStatus = ActivationStatus.Active,

            IsStudentOfAppliedCVMustBeVerified = true,
            StudentOfCVMustHaveActivationStatus = ActivationStatus.Active
        }));
    }

    [HttpGet("{jobOfferId}/subscribe")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> IsStudentSubscribedToJobOffer(Guid jobOfferId)
    {
        return Ok(await Mediator.Send(new IsVerifiedActiveStudentSubscribedToActiveJobOfferWithVerifiedActiveCompanyQuery
        {
            StudentId = AccountInfo!.Id,
            JobOfferId = jobOfferId
        }));
    }

    [HttpPost("{jobOfferId}/subscribe")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SubscribeToJobOffer(Guid jobOfferId)
    {
        await Mediator.Send(new VerifiedActiveStudentSubscribeToActiveJobOfferWithVerifiedActiveCompanyCommand
        {
            StudentId = AccountInfo!.Id,
            JobOfferId = jobOfferId
        });

        return NoContent();
    }

    [HttpDelete("{jobOfferId}/subscribe")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UnsubscribeFromJobOffer(Guid jobOfferId)
    {
        await Mediator.Send(new VerifiedActiveStudentUnsubscribeFromActiveJobOfferWithVerifiedActiveCompanyCommand
        {
            StudentId = AccountInfo!.Id,
            JobOfferId = jobOfferId
        });

        return NoContent();
    }
}
