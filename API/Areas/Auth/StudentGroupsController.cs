using API.Authorize;
using Application.Common.DTO.StudentGroups;
using Application.StudentGroups.Queries.GetBriefStudentGroup;
using Application.StudentGroups.Queries.GetStudentGroups;
using Microsoft.AspNetCore.Mvc;

namespace API.Areas.Auth;

[Authorize]
[Route("api/Auth/[controller]")]
public class StudentGroupsController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BriefStudentGroupDTO>))]
    public async Task<IActionResult> GetStudentGroups(
        [FromQuery] string? searchTerm)
    {
        return Ok(await Sender.Send(new GetBriefStudentGroupsQuery
        {
            SearchTerm = searchTerm ?? string.Empty
        }));
    }

    [HttpGet("{studentGroupId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BriefStudentGroupDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStudentGroup(Guid studentGroupId)
    {
        return Ok(await Sender.Send(new GetBriefStudentGroupQuery(studentGroupId)));
    }
}
