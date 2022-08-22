using Application.Common.Models.Search;
using Application.StudentGroups.Commands.CreateStudentGroup;
using Application.StudentGroups.Commands.DeleteStudentGroup;
using Application.StudentGroups.Commands.UpdateStudentGroup;
using Application.StudentGroups.Queries;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;
using WebUI.DTO.StudentGroup;

namespace WebUI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentGroupsController : ApiControllerBase
{
    /// <summary>
    /// Auth
    /// </summary>
    [HttpGet]
    [Authorize]
    public async Task<IEnumerable<StudentGroupBriefResponse>> GetStudentGroups([FromQuery] SearchParameter searchParameter)
    {
        var result = await Mediator.Send(new GetStudentGroupsQuery
        {
            SearchParameter = searchParameter
        });

        return result.Select(x => new StudentGroupResponse(x));
    }

    /// <summary>
    /// Admin
    /// </summary>
    [HttpDelete("{studentGroupId}")]
    [Authorize("Admin")]
    public async Task<IActionResult> DeleteStudentGroup(Guid studentGroupId)
    {
        await Mediator.Send(new DeleteStudentGroupCommand(studentGroupId));
        return NoContent();
    }

    /// <summary>
    /// Admin
    /// </summary>
    [HttpPost]
    [Authorize("Admin")]
    public async Task<ActionResult<Guid>> CreateStudentGroup(CreateStudentGroupRequest createStudentGroup)
    {
        return await Mediator.Send(new CreateStudentGroupCommand { Name = createStudentGroup.Name });
    }

    /// <summary>
    /// Admin
    /// </summary>
    [HttpPut("{studentGroupId}")]
    [Authorize("Admin")]
    public async Task<IActionResult> UpdateStudentGroup(Guid studentGroupId, UpdateStudentGroupRequest updateStudentGroup)
    {
        await Mediator.Send(new UpdateStudentGroupCommand { Id = studentGroupId, Name = updateStudentGroup.Name });
        return NoContent();
    }
}
