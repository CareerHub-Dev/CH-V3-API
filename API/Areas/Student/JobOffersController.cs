using Application.JobOffers.Commands.VerifiedActiveStudentSubscribeToActiveJobOfferWithVerifiedActiveCompany;
using Application.JobOffers.Commands.VerifiedStudentUnubscribeFromActiveJobOffer;
using Application.JobOffers.Queries;
using Application.JobOffers.Queries.GetAmount;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using API.Authorize;
using Application.Common.DTO.JobOffers;
using Application.JobOffers.Queries.GetJobOffer;

namespace API.Areas.Student;

[Authorize("Student")]
[Route("api/Student/[controller]")]
public class JobOffersController : ApiControllerBase
{
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
