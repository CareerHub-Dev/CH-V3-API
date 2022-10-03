using Application.JobOffers.Commands.CreateJobOffer;
using Application.JobOffers.Commands.DeleteJobOffer;
using Application.JobOffers.Commands.UpdateJobOfferDetail;
using Application.JobOffers.Commands.UpdateJobOfferImage;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;

namespace WebUI.Areas.Admin;

[Authorize("Admin")]
[Route("api/Admin/[controller]")]
public class JobOffersController : ApiControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateJobOffer(CreateJobOfferCommand command)
    {
        var result = await Mediator.Send(command);

        return Ok(result);
    }

    [HttpDelete("{jobOfferId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteJobOffer(Guid jobOfferId)
    {
        await Mediator.Send(new DeleteJobOfferCommand(jobOfferId));

        return NoContent();
    }

    [HttpPut("{jobOfferId}/detail")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateJobOfferDetail(Guid jobOfferId, UpdateJobOfferDetailCommand command)
    {
        if (jobOfferId != command.JobOfferId)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPost("{jobOfferId}/image")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid?))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateJobOfferImage(Guid jobOfferId, IFormFile? file)
    {
        var result = await Mediator.Send(new UpdateJobOfferImageCommand
        {
            JobofferId = jobOfferId,
            Image = file
        });

        return Ok(result);
    }
}
