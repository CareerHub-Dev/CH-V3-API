﻿using Application.JobOffers.Commands.CreateJobOffer;
using Application.JobOffers.Commands.DeleteJobOfferOfCompany;
using Application.JobOffers.Commands.UpdateJobOfferDetailOfCompany;
using Application.JobOffers.Commands.UpdateJobOfferImageOfCompany;
using Microsoft.AspNetCore.Mvc;
using API.Authorize;
using API.DTO.Requests.JobOffers;
using Application.JobOffers.Queries.GetAmount;
using Domain.Enums;

namespace API.Areas.Company;

[Authorize("Company")]
[Route("api/Company/self/[controller]")]
public class JobOffersController : ApiControllerBase
{
    [HttpGet("{jobOfferId}/amount-student-subscribers")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountStudentSubscribersOfSelfJobOffer(Guid jobOfferId)
    {
        return Ok(await Mediator.Send(new GetAmountStudentSubscribersOfCompanyJobOfferWithFilterQuery
        {
            JobOfferId = jobOfferId,
            CompanyId = AccountInfo!.Id,

            IsSubscriberMustBeVerified = true,
            SubscriberMustHaveActivationStatus = ActivationStatus.Active
        }));
    }

    [HttpGet("{jobOfferId}/amount-applied-cvs")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountAppliedCVsOfSelfJobOffer(Guid jobOfferId)
    {
        return Ok(await Mediator.Send(new GetAmountAppliedCVsOfCompanyJobOfferWithFilterQuery
        {
            JobOfferId = jobOfferId,
            CompanyId = AccountInfo!.Id,

            IsStudentOfAppliedCVMustBeVerified = true,
            StudentOfCVMustHaveActivationStatus = ActivationStatus.Active
        }));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateJobOfferForSelfCompany(CreateJobOfferRequest view)
    {
        var result = await Mediator.Send(new CreateJobOfferCommand
        {
            Title = view.Title,
            Overview = view.Overview,
            Requirements = view.Requirements,
            Responsibilities = view.Responsibilities,
            Preferences = view.Preferences,
            Image = view.Image,
            JobType = view.JobType,
            WorkFormat = view.WorkFormat,
            ExperienceLevel = view.ExperienceLevel,
            StartDate = view.StartDate,
            EndDate = view.EndDate,
            JobPositionId = view.JobPositionId,
            TagIds = view.TagIds,

            CompanyId = AccountInfo!.Id
        });

        return Ok(result);
    }

    [HttpDelete("{jobOfferId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteJobOfferOfSelfCompany(Guid jobOfferId)
    {
        await Mediator.Send(new DeleteJobOfferOfCompanyCommand(jobOfferId, AccountInfo!.Id));

        return NoContent();
    }

    [HttpPut("{jobOfferId}/detail")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateJobOfferDetailOfSelfCompany(Guid jobOfferId, UpdateJobOfferDetailRequest view)
    {
        if (jobOfferId != view.JobOfferId)
        {
            return BadRequest();
        }

        await Mediator.Send(new UpdateJobOfferDetailOfCompanyCommand
        {
            JobOfferId = view.JobOfferId,
            Title = view.Title,
            Overview = view.Overview,
            Requirements = view.Requirements,
            Responsibilities = view.Responsibilities,
            Preferences = view.Preferences,
            JobType = view.JobType,
            WorkFormat = view.WorkFormat,
            ExperienceLevel = view.ExperienceLevel,
            StartDate = view.StartDate,
            EndDate = view.EndDate,
            JobPositionId = view.JobPositionId,
            TagIds = view.TagIds,

            CompanyId = AccountInfo!.Id
        });

        return NoContent();
    }

    [HttpPost("{jobOfferId}/image")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid?))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateJobOfferImageOfSelfCompany(Guid jobOfferId, IFormFile? file)
    {
        var result = await Mediator.Send(new UpdateJobOfferImageOfCompanyCommand 
        { 
            CompanyId = AccountInfo!.Id, 

            JobofferId = jobOfferId,
            Image = file 
        });

        return Ok(result);
    }
}
