using Application.Common.DTO.JobPositions;
using Application.JobPositions.Queries.GetJobPositions;
using Application.Tags.Queries.GetJobPosition;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;

namespace WebUI.Areas.Auth;

[Authorize]
[Route("api/Auth/[controller]")]
public class JobPositionsController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<BriefJobPositionDTO>))]
    public async Task<IActionResult> GetJobPositions([FromQuery] GetBriefJobPositionsWithSearchQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpGet("{jobPositionId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BriefJobPositionDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetJobPosition(Guid jobPositionId)
    {
        return Ok(await Mediator.Send(new GetBriefJobPositionQuery(jobPositionId)));
    }
}
