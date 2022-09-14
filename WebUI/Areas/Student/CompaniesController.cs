using Application.Companies.Commands.VerifiedStudentSubscribeToVerifiedCompany;
using Application.Companies.Commands.VerifiedStudentUnsubscribeFromVerifiedCompany;
using Application.Companies.Queries.GetAmount;
using Application.Companies.Queries.GetCompany;
using Application.Companies.Queries.Models;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;

namespace WebUI.Areas.Student;

[Authorize("Student")]
[Route("api/Student/[controller]")]
public class CompaniesController : ApiControllerBase
{
    [HttpGet("{companyId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FollowedCompanyDetailedDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCompany(Guid companyId)
    {
        return Ok(await Mediator.Send(new GetFollowedCompanyDetailedForFollowerStudentWithFilterQuery
        {
            FollowerStudentId = AccountInfo!.Id,
            IsFollowerStudentMustBeVerified = true,
            CompanyId = companyId,
            IsCompanyMustBeVerified = true
        }));
    }

    [HttpGet("{companyId}/amountSubscribers")]
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

    [HttpGet("{companyId}/amountJobOffers")]
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

    [HttpPost("{companyId}/subscribe")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SubscribeToCompany(Guid companyId)
    {
        await Mediator.Send(new VerifiedStudentSubscribeToVerifiedCompanyCommand
        {
            StudentId = AccountInfo!.Id,
            CompanytId = companyId
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
            CompanytId = companyId
        });

        return NoContent();
    }
}
