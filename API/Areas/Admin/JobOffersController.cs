using API.Authorize;
using Application.Common.DTO.JobOffers;
using Application.Common.Enums;
using Application.JobOffers.Commands.CreateJobOffer;
using Application.JobOffers.Commands.DeleteJobOffer;
using Application.JobOffers.Commands.UpdateJobOfferDetail;
using Application.JobOffers.Commands.UpdateJobOfferImage;
using Application.JobOffers.Queries.GetAmountAppliedCVsOfJobOffer;
using Application.JobOffers.Queries.GetAmountStudentSubscribersOfJobOffer;
using Application.JobOffers.Queries.GetJobOffer;
using Microsoft.AspNetCore.Mvc;

namespace API.Areas.Admin;

[Authorize(Role.Admin, Role.SuperAdmin)]
[Route("api/Admin/[controller]")]
public class JobOffersController : ApiControllerBase
{
    [HttpGet("{jobOfferId}/amount-student-subscribers")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountStudentSubscribersOfJobOffer(Guid jobOfferId)
    {
        return Ok(await Sender.Send(new GetAmountStudentSubscribersOfJobOfferQuery
        {
            JobOfferId = jobOfferId
        }));
    }

    [HttpGet("{jobOfferId}/amount-applied-cvs")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountAppliedCVsOfJobOffer(Guid jobOfferId)
    {
        return Ok(await Sender.Send(new GetAmountAppliedCVsOfJobOfferQuery
        {
            JobOfferId = jobOfferId
        }));
    }

    [HttpGet("{jobOfferId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JobOfferDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetJobOffer(Guid jobOfferId)
    {
        return Ok(await Sender.Send(new GetJobOfferQuery
        {
            JobOfferId = jobOfferId
        }));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateJobOffer([FromForm] CreateJobOfferCommand command)
    {
        var result = await Sender.Send(command);

        return Ok(result);
    }

    [HttpDelete("{jobOfferId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteJobOffer(Guid jobOfferId)
    {
        await Sender.Send(new DeleteJobOfferCommand(jobOfferId));

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

        await Sender.Send(command);

        return NoContent();
    }

    [HttpPost("{jobOfferId}/image")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateJobOfferImage(Guid jobOfferId, IFormFile? file)
    {
        var result = await Sender.Send(new UpdateJobOfferImageCommand
        {
            JobofferId = jobOfferId,
            Image = file
        });

        return Ok(result);
    }
}
