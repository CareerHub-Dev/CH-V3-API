using Application.JobOffers.Commands.VerifiedActiveStudentSubscribeToActiveJobOfferWithVerifiedActiveCompany;
using Application.JobOffers.Commands.VerifiedStudentUnubscribeFromActiveJobOffer;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;

namespace WebUI.Areas.Student;

[Authorize("Student")]
[Route("api/Student/[controller]")]
public class JobOffersController : ApiControllerBase
{
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
