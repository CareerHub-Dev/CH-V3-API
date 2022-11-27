using API.Authorize;
using Application.Common.DTO.JobPositions;
using Application.JobPositions.Queries.GetBriefJobPosition;
using Application.JobPositions.Queries.GetBriefJobPositions;
using Microsoft.AspNetCore.Mvc;

namespace API.Areas.Auth;

[Authorize]
[Route("api/Auth/[controller]")]
public class JobPositionsController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BriefJobPositionDTO>))]
    public async Task<IActionResult> GetJobPositions(
        [FromQuery] string? search)
    {
        return Ok(await Sender.Send(new GetBriefJobPositionsQuery
        {
            SearchTerm = search ?? string.Empty
        }));
    }

    [HttpGet("{jobPositionId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BriefJobPositionDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetJobPosition(Guid jobPositionId)
    {
        return Ok(await Sender.Send(new GetBriefJobPositionQuery(jobPositionId)));
    }
}
