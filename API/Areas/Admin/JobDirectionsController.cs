using API.Authorize;
using Application.Common.DTO.JobDirection;
using Application.Common.Enums;
using Application.JobDirections.Commands.CreateJobDirection;
using Application.JobDirections.Commands.DeleteJobOffer;
using Application.JobDirections.Commands.UpdateJobDirection;
using Application.JobDirections.Queries.GetJobDirection;
using Application.JobDirections.Queries.GetJobDirections;
using Microsoft.AspNetCore.Mvc;

namespace API.Areas.Admin;

[Authorize(Role.Admin, Role.SuperAdmin)]
[Route("api/Admin/[controller]")]
public class JobDirectionsController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<JobDirectionDTO>))]
    public async Task<IActionResult> GetJobDirections(
        [FromQuery] string? search)
    {
        var result = await Sender.Send(new GetJobDirectionsQuery
        {
            SearchTerm = search ?? string.Empty,
        });

        return Ok(result);
    }

    [HttpGet("{jobDirectionId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JobDirectionDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetJobDirection(Guid jobDirectionId)
    {
        return Ok(await Sender.Send(new GetJobDirectionQuery(jobDirectionId)));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    public async Task<IActionResult> CreateJobDirection(CreateJobDirectionCommand command)
    {
        var result = await Sender.Send(command);

        return Ok(result);
    }

    [HttpPut("{jobDirectionId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateJobDirection(Guid jobDirectionId, UpdateJobDirectionCommand command)
    {
        if (jobDirectionId != command.JobDirectionId)
        {
            return BadRequest();
        }

        await Sender.Send(command);

        return NoContent();
    }

    [HttpDelete("{jobDirectionId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteJobPosition(Guid jobDirectionId)
    {
        await Sender.Send(new DeleteJobDirectionCommand(jobDirectionId));

        return NoContent();
    }
}
