using API.Authorize;
using Application.Common.DTO.JobPositions;
using Application.Common.Enums;
using Application.JobDirections.Commands.CreateJobDirection;
using Application.JobDirections.Commands.DeleteJobOffer;
using Application.JobDirections.Commands.UpdateJobDirection;
using Application.JobPositions.Commands.CreateJobPosition;
using Application.JobPositions.Commands.DeleteJobPosition;
using Application.JobPositions.Commands.UpdateJobPosition;
using Application.JobPositions.Queries.GetBriefJobPosition;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Areas.Admin;

[Authorize(Role.Admin, Role.SuperAdmin)]
[Route("api/Admin/[controller]")]
public class JobPositionsController : ApiControllerBase
{
    [HttpGet("{jobPositionId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JobPositionDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetJobPosition(Guid jobPositionId)
    {
        return Ok(await Sender.Send(new GetJobPositionQuery(jobPositionId)));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    public async Task<IActionResult> CreateJobPosition(CreateJobPositionCommand command)
    {
        var result = await Sender.Send(command);

        return Ok(result);
    }

    [HttpPut("{jobPositionId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateJobPosition(Guid jobPositionId, UpdateJobPositionCommand command)
    {
        if (jobPositionId != command.JobPositionId)
        {
            return BadRequest();
        }

        await Sender.Send(command);

        return NoContent();
    }

    [HttpDelete("{jobPositionId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteJobPosition(Guid jobPositionId)
    {
        await Sender.Send(new DeleteJobPositionCommand(jobPositionId));

        return NoContent();
    }
}
