using Application.Common.Models.StudentGroup;
using Application.StudentGroups.Queries;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;

namespace WebUI.Areas.Auth;

[Authorize]
[Route("api/Auth/[controller]")]
public class StudentGroupsController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<BriefStudentGroupDTO>))]
    public async Task<IActionResult> GetStudentGroups([FromQuery] GetBriefStudentGroupsWithSearchQuery query)
    {
        return Ok(await Mediator.Send(query));
    }
}
