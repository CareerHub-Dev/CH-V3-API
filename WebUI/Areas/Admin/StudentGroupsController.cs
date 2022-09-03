using Application.StudentGroups.Commands.CreateStudentGroup;
using Application.StudentGroups.Commands.DeleteStudentGroup;
using Application.StudentGroups.Commands.UpdateStudentGroup;
using Application.StudentGroups.Queries;
using Application.StudentGroups.Queries.Models;
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
    public async Task<IEnumerable<StudentGroupDTO>> GetStudentGroups([FromQuery] GetStudentGroupsWithPaginationWithSearchQuery query)
    {
        var result = await Mediator.Send(query);

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return result;
    }

    [HttpDelete("{studentGroupId}")]
    public async Task<IActionResult> DeleteStudentGroup(Guid studentGroupId)
    {
        await Mediator.Send(new DeleteStudentGroupCommand(studentGroupId));

        return NoContent();
    }

    [HttpPost]
    public async Task<Guid> CreateStudentGroup(CreateStudentGroupCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{studentGroupId}")]
    public async Task<IActionResult> UpdateStudentGroup(Guid studentGroupId, UpdateStudentGroupCommand command)
    {
        if(studentGroupId != command.StudentGroupId)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }
}
