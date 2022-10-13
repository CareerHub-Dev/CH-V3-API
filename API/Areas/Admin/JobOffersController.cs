using Application.JobOffers.Commands.CreateJobOffer;
using Application.JobOffers.Commands.DeleteJobOffer;
using Application.JobOffers.Commands.UpdateJobOfferDetail;
using Application.JobOffers.Commands.UpdateJobOfferImage;
using Application.JobOffers.Queries.GetAmount;
using Application.Students.Queries.GetAmount;
using Microsoft.AspNetCore.Mvc;
using API.Authorize;
using Application.Common.DTO.StudentGroups;
using Application.Tags.Queries.GetStudentGroup;
using Application.JobOffers.Queries.GetJobOffer;
using Application.Common.DTO.JobOffers;
using Domain.Entities;

namespace API.Areas.Admin;

[Authorize("Admin", "SuperAdmin")]
[Route("api/Admin/[controller]")]
public class JobOffersController : ApiControllerBase
{
    [HttpGet("{jobOfferId}/amount-student-subscribers")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountStudentSubscribersOfJobOffer(Guid jobOfferId)
    {
        return Ok(await Mediator.Send(new GetAmountStudentSubscribersOfJobOfferWithFilterQuery
        {
            JobOfferId = jobOfferId
        }));
    }

    [HttpGet("{jobOfferId}/amount-applied-cvs")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountAppliedCVsOfJobOffer(Guid jobOfferId)
    {
        return Ok(await Mediator.Send(new GetAmountAppliedCVsOfJobOfferWithFilterQuery
        {
            JobOfferId = jobOfferId
        }));
    }

    [HttpGet("{jobOfferId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JobOfferDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetJobOffer(Guid jobOfferId)
    {
        return Ok(await Mediator.Send(new GetJobOfferWithFilterQuery
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
