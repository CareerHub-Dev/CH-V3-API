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
    public async Task<IEnumerable<JobPositionBriefDTO>> GetJobPositions([FromQuery] GetJobPositionBriefsWithSearchQuery query)
    {
        return await Mediator.Send(query);
    }
}
