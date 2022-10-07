using Application.JobOffers.Commands.VerifiedActiveStudentSubscribeToActiveJobOfferWithVerifiedActiveCompany;
using Application.JobOffers.Commands.VerifiedStudentUnubscribeFromActiveJobOffer;
using Application.JobOffers.Queries;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;

namespace WebUI.Areas.Student;

[Authorize("Student")]
[Route("api/Student/[controller]")]
public class JobOffersController : ApiControllerBase
{
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
