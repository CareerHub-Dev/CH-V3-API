using Application.Common.DTO.StudentGroups;
using Application.StudentGroups.Commands.CreateStudentGroup;
using Application.StudentGroups.Commands.DeleteStudentGroup;
using Application.StudentGroups.Commands.UpdateStudentGroup;
using Application.StudentGroups.Queries.GetStudentGroups;
using Application.Tags.Queries.GetStudentGroup;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebUI.Authorize;

namespace WebUI.Areas.Admin;

[Authorize("Admin", "SuperAdmin")]
[Route("api/Admin/[controller]")]
public class StudentGroupsController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<StudentGroupDTO>))]
    public async Task<IActionResult> GetStudentGroups([FromQuery] GetStudentGroupsWithPaginationWithSearchQuery query)
    {
        var result = await Mediator.Send(query);

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("{studentGroupId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentGroupDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStudentGroup(Guid studentGroupId)
    {
        return Ok(await Mediator.Send(new GetStudentGroupQuery(studentGroupId)));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Guid))]
    public async Task<IActionResult> CreateStudentGroup(CreateStudentGroupCommand command)
    {
        var result = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetStudentGroup), new { studentGroupId = result }, result);
    }

    [HttpPut("{studentGroupId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateStudentGroup(Guid studentGroupId, UpdateStudentGroupCommand command)
    {
        if (studentGroupId != command.StudentGroupId)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{studentGroupId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteStudentGroup(Guid studentGroupId)
    {
        await Mediator.Send(new DeleteStudentGroupCommand(studentGroupId));

        return NoContent();
    }
}
