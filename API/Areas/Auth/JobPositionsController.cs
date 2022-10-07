using Application.Common.DTO.JobPositions;
using Application.JobPositions.Queries.GetJobPositions;
using Application.Tags.Queries.GetJobPosition;
using Microsoft.AspNetCore.Mvc;
using API.Authorize;

namespace API.Areas.Auth;

[Authorize]
[Route("api/Auth/[controller]")]
public class JobPositionsController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BriefJobPositionDTO>))]
    public async Task<IActionResult> GetJobPositions(
        [FromQuery] string? searchTerm)
    {
        return Ok(await Mediator.Send(new GetBriefJobPositionsWithSearchQuery
        {
            SearchTerm = searchTerm ?? string.Empty
        }));
    }

    [HttpGet("{jobPositionId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BriefJobPositionDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetJobPosition(Guid jobPositionId)
    {
        return Ok(await Mediator.Send(new GetBriefJobPositionQuery(jobPositionId)));
    }
}
