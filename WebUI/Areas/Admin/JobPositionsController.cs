using Application.JobPositions.Commands.CreateJobPosition;
using Application.JobPositions.Commands.DeleteJobPosition;
using Application.JobPositions.Commands.UpdateJobPosition;
using Application.JobPositions.Queries;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebUI.Authorize;

namespace WebUI.Areas.Admin;

[Authorize("Admin", "SuperAdmin")]
[Route("api/Admin/[controller]")]
public class JobPositionsController : ApiControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<JobPositionDTO>> GetJobPositions([FromQuery] GetJobPositionsWithPaginationWithSearchQuery query)
    {
        var result = await Mediator.Send(query);

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return result;
    }

    [HttpDelete("{jobPositionId}")]
    public async Task<IActionResult> DeleteJobPosition(Guid jobPositionId)
    {
        await Mediator.Send(new DeleteJobpositionCommand(jobPositionId));

        return NoContent();
    }

    [HttpPost]
    public async Task<Guid> CreateJobPosition(CreateJobPositionCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{jobPositionId}")]
    public async Task<IActionResult> UpdateJobPosition(Guid jobPositionId, UpdateJobPositionCommand command)
    {
        if (jobPositionId != command.JobPositionId)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }
}
