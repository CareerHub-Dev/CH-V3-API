﻿using API.Authorize;
using API.DTO.Requests.JobOffers;
using API.DTO.Responses;
using Application.Common.DTO.CVs;
using Application.Common.DTO.JobOffers;
using Application.Common.Enums;
using Application.CVs.Queries.GetBriefCVWithStatussOfJobOfferWithPaging;
using Application.JobOffers.Commands.CreateJobOffer;
using Application.JobOffers.Commands.DeleteJobOfferOfCompany;
using Application.JobOffers.Commands.UpdateJobOfferDetailOfCompany;
using Application.JobOffers.Commands.UpdateJobOfferImageOfCompany;
using Application.JobOffers.Queries.GetAmountAppliedCVsOfCompanyJobOffer;
using Application.JobOffers.Queries.GetAmountStudentSubscribersOfCompanyJobOffer;
using Application.JobOffers.Queries.GetDetiledJobOffersWithStatsOfCompanyWithPaging;
using Application.JobOffers.Queries.GetJobOfferOfCompany;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using JobOfferStatsFilter = Application.JobOffers.Queries.Models.StatsFilter;

namespace API.Areas.Company;

[Authorize(Role.Company)]
[Route("api/Company/self/[controller]")]
public class JobOffersController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<DetiledJobOfferWithStatsDTO>))]
    public async Task<IActionResult> GetJobOffersOfSelfCompany(
        [FromQuery] string? order,
        [FromQuery] string? search,
        [FromQuery] bool? active,
        [FromQuery] JobType? jobType,
        [FromQuery] WorkFormat? workFormat,
        [FromQuery] ExperienceLevel? experienceLevel,
        [FromQuery] Guid? jobPositionId,
        [FromQuery] List<Guid>? tagIds,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Sender.Send(new GetDetiledJobOffersWithStatsOfCompanyWithPagingQuery
        {
            CompanyId = AccountInfo!.Id,

            PageNumber = pageNumber,
            PageSize = pageSize,

            SearchTerm = search ?? string.Empty,

            IsJobOfferMustBeActive = active,
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

    [HttpGet("{jobOfferId}/amount-student-subscribers")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountStudentSubscribersOfSelfJobOffer(Guid jobOfferId)
    {
        return Ok(await Sender.Send(new GetAmountStudentSubscribersOfCompanyJobOfferQuery
        {
            JobOfferId = jobOfferId,
            CompanyId = AccountInfo!.Id,

            IsSubscriberMustBeVerified = true,
        }));
    }

    [HttpGet("{jobOfferId}/amount-applied-cvs")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountAppliedCVsOfSelfJobOffer(Guid jobOfferId)
    {
        return Ok(await Sender.Send(new GetAmountAppliedCVsOfCompanyJobOfferQuery
        {
            JobOfferId = jobOfferId,
            CompanyId = AccountInfo!.Id,

            IsStudentOfAppliedCVMustBeVerified = true,
        }));
    }

    [HttpGet("{jobOfferId}/CVs")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BriefCVWithStatusAndStudentDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAppliedCVsByJobOffer(
        Guid jobOfferId,
        [FromQuery] string? order,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Sender.Send(new GetBriefCVWithStatusAndStudentWithPagingQuery
        {
            JobOfferId = jobOfferId,
            PageNumber = pageNumber,
            PageSize = pageSize,
            OrderByExpression = order ?? "Created ASC"
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("{jobOfferId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JobOfferDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetJobOffer(Guid jobOfferId)
    {
        return Ok(await Sender.Send(new GetJobOfferOfCompanyQuery
        {
            JobOfferId = jobOfferId,
            CompanyId = AccountInfo!.Id
        }));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateJobOfferForSelfCompany(CreateOwnJobOfferRequest request)
    {
        var result = await Sender.Send(new CreateJobOfferCommand
        {
            Title = request.Title,
            Overview = request.Overview,
            Requirements = request.Requirements,
            Responsibilities = request.Responsibilities,
            Preferences = request.Preferences,
            JobType = request.JobType,
            WorkFormat = request.WorkFormat,
            ExperienceLevel = request.ExperienceLevel,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            JobPositionId = request.JobPositionId,
            TagIds = request.TagIds,
            CompanyId = AccountInfo!.Id
        });

        return Ok(result);
    }

    [HttpDelete("{jobOfferId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteJobOfferOfSelfCompany(Guid jobOfferId)
    {
        await Sender.Send(new DeleteJobOfferOfCompanyCommand(jobOfferId, AccountInfo!.Id));

        return NoContent();
    }

    [HttpPut("{jobOfferId}/detail")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateJobOfferDetailOfSelfCompany(Guid jobOfferId, UpdateOwnJobOfferDetailRequest request)
    {
        await Sender.Send(new UpdateJobOfferDetailOfCompanyCommand
        {
            JobOfferId = jobOfferId,
            Title = request.Title,
            Overview = request.Overview,
            Requirements = request.Requirements,
            Responsibilities = request.Responsibilities,
            Preferences = request.Preferences,
            JobType = request.JobType,
            WorkFormat = request.WorkFormat,
            ExperienceLevel = request.ExperienceLevel,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            JobPositionId = request.JobPositionId,
            TagIds = request.TagIds,
            CompanyId = AccountInfo!.Id
        });

        return NoContent();
    }

    [HttpPost("{jobOfferId}/image")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImageResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateJobOfferImageOfSelfCompany(Guid jobOfferId, IFormFile? file)
    {
        var result = await Sender.Send(new UpdateJobOfferImageOfCompanyCommand
        {
            CompanyId = AccountInfo!.Id,

            JobofferId = jobOfferId,
            Image = file
        });

        return Ok(new ImageResponse { Route = result });
    }
}
