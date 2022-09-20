using Application.Common.DTO.StudentGroups;
using Application.StudentGroups.Queries.GetStudentGroups;
using Application.Tags.Queries.GetStudentGroup;
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

    [HttpGet("{studentGroupId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BriefStudentGroupDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStudentGroup(Guid studentGroupId)
    {
        return Ok(await Mediator.Send(new GetBriefStudentGroupQuery(studentGroupId)));
    }
}
