﻿using Application.Common.DTO.Companies;
using Application.Common.DTO.CompanyLinks;
using Application.Companies.Commands.VerifiedActiveStudentSubscribeToVerifiedActiveCompany;
using Application.Companies.Commands.VerifiedActiveStudentUnsubscribeFromVerifiedActiveCompany;
using Application.Companies.Queries;
using Application.Companies.Queries.GetAmount;
using Application.Companies.Queries.GetCompanies;
using Application.Companies.Queries.GetCompany;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using API.Authorize;
using Application.Common.DTO.JobOffers;
using Application.JobOffers.Queries.GetJobOffersOfCompany;

using CompanyStatsFilter = Application.Companies.Queries.Models.StatsFilter;
using JobOfferStatsFilter = Application.JobOffers.Queries.Models.StatsFilter;

namespace API.Areas.Student;

[Authorize("Student")]
[Route("api/Student/[controller]")]
public class CompaniesController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CompanyWithStatsDTO>))]
    public async Task<IActionResult> GetCompanies(
        [FromQuery] string? orderByExpression,
        [FromQuery] string? searchTerm,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Mediator.Send(new GetFollowedDetailedCompaniesWithStatsForFollowerStudentWithPaginationWithSearchWithFilterWithSortQuery
        {
            FollowerStudentId = AccountInfo!.Id,

            PageNumber = pageNumber,
            PageSize = pageSize,

            SearchTerm = searchTerm ?? string.Empty,

            IsCompanyMustBeVerified = true,
            CompanyMustHaveActivationStatus = ActivationStatus.Active,

            StatsFilter = new CompanyStatsFilter
            {
                IsJobOfferMustBeActive = true,

                IsSubscriberMustBeVerified = true,
                SubscriberMustHaveActivationStatus = ActivationStatus.Active
            },

            OrderByExpression = orderByExpression ?? "Name",
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("{companyId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FollowedDetailedCompanyDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCompany(Guid companyId)
    {
        return Ok(await Mediator.Send(new GetDetailedCompanyWithFilterQuery
        {
            CompanyId = companyId,
            IsCompanyMustBeVerified = true,
            CompanyMustHaveActivationStatus = ActivationStatus.Active
        }));
    }

    [HttpGet("{companyId}/amount-subscribers")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountStudentSubscribersOfCompany(Guid companyId)
    {
        return Ok(await Mediator.Send(new GetAmountStudentSubscribersOfCompanyWithFilterQuery
        {
            CompanyId = companyId,
            IsCompanyMustBeVerified = true,
            CompanyMustHaveActivationStatus = ActivationStatus.Active,

            IsSubscriberMustBeVerified = true,
            SubscriberMustHaveActivationStatus = ActivationStatus.Active
        }));
    }

    [HttpGet("{companyId}/amount-jobOffers")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountJobOffersOfCompany(Guid companyId)
    {
        return Ok(await Mediator.Send(new GetAmountJobOffersOfCompanyWithFilterQuery
        {
            CompanyId = companyId,
            IsCompanyMustBeVerified = true,
            CompanyMustHaveActivationStatus = ActivationStatus.Active,

            IsJobOfferMustBeActive = true
        }));
    }

    [HttpGet("{companyId}/subscribe")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> IsStudentSubscribedToCompany(Guid companyId)
    {
        return Ok(await Mediator.Send(new IsVerifiedActiveStudentSubscribedToVerifiedActiveCompanyQuery
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
        await Mediator.Send(new VerifiedActiveStudentSubscribeToVerifiedActiveCompanyCommand
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
        await Mediator.Send(new VerifiedActiveStudentUnsubscribeFromVerifiedActiveCompanyCommand
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
        var result = await Mediator.Send(new GetFollowedDetiledJobOffersWithStatsOfCompanyForFollowerStudentWithPaginationWithSearchWithFilterWithSortQuery
        {
            FollowerStudentId = AccountInfo!.Id,

            CompanyId = companyId,
            IsCompanyOfJobOfferMustBeVerified = true,
            CompanyOfJobOfferMustHaveActivationStatus = ActivationStatus.Active,

            PageNumber = pageNumber,
            PageSize = pageSize,

            SearchTerm = searchTerm ?? string.Empty,

            IsJobOfferMustBeActive = true,
            MustHaveWorkFormat = mustHaveWorkFormat,
            MustHaveJobType = mustHaveJobType,
            MustHaveExperienceLevel = mustHaveExperienceLevel,
            MustHaveJobPositionId = mustHavejobPositionId,
            MustHaveTagIds = mustHaveTagIds,

            StatsFilter = new JobOfferStatsFilter
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
}
