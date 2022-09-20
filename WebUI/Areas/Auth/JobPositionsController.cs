using Application.Common.Models.JobPosition;
using Application.JobPositions.Queries;
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
}
