using Application.StudentGroups.Commands.CreateStudentGroup;
using Application.StudentGroups.Commands.DeleteStudentGroup;
using Application.StudentGroups.Commands.UpdateStudentGroup;
using Application.StudentGroups.Queries;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebUI.Authorize;
using WebUI.Common.Models;
using WebUI.Common.Models.StudentGroup;

namespace WebUI.Areas.Admin;

[Authorize("Admin")]
[Route("api/Admin/[controller]")]
public class StudentGroupsController : ApiControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<StudentGroupResponse>> GetStudentGroups(
        [FromQuery] PaginationParameters paginationParameters,
        [FromQuery] SearchParameter searchParameter)
    {
        var result = await Mediator.Send(new GetStudentGroupsWithPaginationWithSearchQuery
        {
            PageSize = paginationParameters.PageSize,
            PageNumber = paginationParameters.PageNumber,
            SearchTerm = searchParameter.SearchTerm,
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return result.Select(x => new StudentGroupResponse(x));
    }

    [HttpGet("{studentGroupId}")]
    public async Task<StudentGroupResponse> GetStudentGroup(Guid studentGroupId)
    {
        var result = await Mediator.Send(new GetStudentGroupQuery(studentGroupId));

        return new StudentGroupResponse(result);
    }

    [HttpDelete("{studentGroupId}")]
    public async Task<IActionResult> DeleteStudentGroup(Guid studentGroupId)
    {
        await Mediator.Send(new DeleteStudentGroupCommand(studentGroupId));
        return NoContent();
    }

    [HttpPost]
    public async Task<Guid> CreateStudentGroup(CreateStudentGroupRequest createStudentGroup)
    {
        return await Mediator.Send(new CreateStudentGroupCommand { Name = createStudentGroup.Name });
    }

    [HttpPut("{studentGroupId}")]
    public async Task<IActionResult> UpdateStudentGroup(Guid studentGroupId, UpdateStudentGroupRequest updateStudentGroup)
    {
        await Mediator.Send(new UpdateStudentGroupCommand { StudentGroupId = studentGroupId, Name = updateStudentGroup.Name });
        return NoContent();
    }
}
