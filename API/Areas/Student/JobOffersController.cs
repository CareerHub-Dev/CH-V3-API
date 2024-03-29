﻿using API.Authorize;
using Application.Common.DTO.JobOfferReviews;
using Application.Common.DTO.JobOffers;
using Application.Common.Enums;
using Application.JobOffers.Commands.VerifiedStudentSubscribeToActiveJobOfferWithVerifiedCompany;
using Application.JobOffers.Commands.VerifiedStudentUnsubscribeFromActiveJobOfferWithVerifiedCompany;
using Application.JobOffers.Queries.GetAmountAppliedCVsOfJobOffer;
using Application.JobOffers.Queries.GetAmountStudentSubscribersOfJobOffer;
using Application.JobOffers.Queries.GetFollowedDetiledJobOffersWithStatsForFollowerStudentWithPaging;
using Application.JobOffers.Queries.GetJobOffer;
using Application.JobOffers.Queries.GetRecomendedFollowedDetiledJobOffersWithStatsForFollowerStudentWithPaging;
using Application.JobOffers.Queries.IsVerifiedStudentSubscribedToActiveJobOfferWithVerifiedCompany;
using Application.JobOffers.Queries.Models;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Areas.Student;

[Authorize(Role.Student)]
[Route("api/Student/[controller]")]
public class JobOffersController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FollowedDetiledJobOfferWithStatsDTO>))]
    public async Task<IActionResult> GetJobOffers(
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
        var result = await Sender.Send(new GetFollowedDetiledJobOffersWithStatsForFollowerStudentWithPagingQuery
        {
            FollowerStudentId = AccountInfo!.Id,

            PageNumber = pageNumber,
            PageSize = pageSize,

            SearchTerm = search ?? string.Empty,

            IsJobOfferMustBeActive = true,
            MustHaveWorkFormat = workFormat,
            MustHaveJobType = jobType,
            MustHaveExperienceLevel = experienceLevel,
            MustHaveJobPositionId = jobPositionId,
            MustHaveTagIds = tagIds,
            IsCompanyOfJobOfferMustBeVerified = true,

            StatsFilter = new StatsFilter
            {
                IsStudentOfAppliedCVMustBeVerified = true,

                IsSubscriberMustBeVerified = true,
            },

            OrderByExpression = order ?? "StartDate",
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("recomended")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FollowedDetiledJobOfferWithStatsDTO>))]
    public async Task<IActionResult> GetRecomendedJobOffers(
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
        var result = await Sender.Send(new GetRecomendedFollowedDetiledJobOffersWithStatsForFollowerStudentWithPagingQuery
        {
            FollowerStudentId = AccountInfo!.Id,

            PageNumber = pageNumber,
            PageSize = pageSize,

            SearchTerm = search ?? string.Empty,

            IsJobOfferMustBeActive = true,
            MustHaveWorkFormat = workFormat,
            MustHaveJobType = jobType,
            MustHaveExperienceLevel = experienceLevel,
            MustHaveJobPositionId = jobPositionId,
            MustHaveTagIds = tagIds,
            IsCompanyOfJobOfferMustBeVerified = true,

            StatsFilter = new StatsFilter
            {
                IsStudentOfAppliedCVMustBeVerified = true,

                IsSubscriberMustBeVerified = true,
            },

            OrderByExpression = order ?? "StartDate",
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("{jobOfferId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JobOfferDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetJobOffer(Guid jobOfferId)
    {
        return Ok(await Sender.Send(new GetJobOfferQuery
        {
            JobOfferId = jobOfferId,
            IsJobOfferMustBeActive = true,
            IsCompanyOfJobOfferMustBeVerified = true,
        }));
    }

    [HttpGet("{jobOfferId}/amount-student-subscribers")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountStudentSubscribersOfJobOffer(Guid jobOfferId)
    {
        return Ok(await Sender.Send(new GetAmountStudentSubscribersOfJobOfferQuery
        {
            JobOfferId = jobOfferId,
            IsJobOfferMustBeActive = true,
            IsCompanyOfJobOfferMustBeVerified = true,

            IsSubscriberMustBeVerified = true,
        }));
    }

    [HttpGet("{jobOfferId}/amount-applied-cvs")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountAppliedCVsOfJobOffer(Guid jobOfferId)
    {
        return Ok(await Sender.Send(new GetAmountAppliedCVsOfJobOfferQuery
        {
            JobOfferId = jobOfferId,
            IsJobOfferMustBeActive = true,
            IsCompanyOfJobOfferMustBeVerified = true,

            IsStudentOfAppliedCVMustBeVerified = true,
        }));
    }

    [HttpGet("{jobOfferId}/subscribe")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> IsStudentSubscribedToJobOffer(Guid jobOfferId)
    {
        return Ok(await Sender.Send(new IsVerifiedStudentSubscribedToActiveJobOfferWithVerifiedCompanyQuery
        {
            StudentId = AccountInfo!.Id,
            JobOfferId = jobOfferId
        }));
    }

    [HttpGet("{jobOfferId}/applied")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> StudentAppliedToJobOffer(Guid jobOfferId)
    {
        return Ok(await Sender.Send(new DidStudentApplyToJobOfferQuery
        {
            StudentId = AccountInfo!.Id,
            JobOfferId = jobOfferId
        }));
    }

    [HttpGet("{jobOfferId}/applications")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<JobOfferReviewDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> StudentJobOfferApplications(Guid jobOfferId)
    {
        return Ok(await Sender.Send(new GetStudentApplicationsToJobOfferQuery
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
        await Sender.Send(new VerifiedStudentSubscribeToActiveJobOfferWithVerifiedCompanyCommand
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
        await Sender.Send(new VerifiedStudentUnsubscribeFromActiveJobOfferWithVerifiedCompanyCommand
        {
            StudentId = AccountInfo!.Id,
            JobOfferId = jobOfferId
        });

        return NoContent();
    }
}
