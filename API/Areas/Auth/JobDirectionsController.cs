using API.Authorize;
using Application.Common.DTO.JobDirection;
using Application.JobDirections.Queries.GetJobDirection;
using Application.JobDirections.Queries.GetJobDirections;
using Microsoft.AspNetCore.Mvc;

namespace API.Areas.Auth;

[Authorize]
[Route("api/Auth/[controller]")]
public class JobDirectionsController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<JobDirectionDTO>))]
    public async Task<IActionResult> GetJobDirections(
        [FromQuery] string? search)
    {
        return Ok(await Sender.Send(new GetJobDirectionsQuery
        {
            SearchTerm = search ?? string.Empty
        }));
    }

    [HttpGet("{jobDirectionId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JobDirectionDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetJobDirection(Guid jobDirectionId)
    {
        return Ok(await Sender.Send(new GetJobDirectionQuery(jobDirectionId)));
    }
}
