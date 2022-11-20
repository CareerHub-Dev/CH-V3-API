using API.Authorize;
using Application.Common.DTO.StudentGroups;
using Application.Common.Enums;
using Application.StudentGroups.Commands.CreateStudentGroup;
using Application.StudentGroups.Commands.DeleteStudentGroup;
using Application.StudentGroups.Commands.UpdateStudentGroup;
using Application.StudentGroups.Queries.GetStudentGroupsWithPaging;
using Application.Tags.Queries.GetStudentGroup;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Areas.Admin;

[Authorize(Role.Admin, Role.SuperAdmin)]
[Route("api/Admin/[controller]")]
public class StudentGroupsController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<StudentGroupDTO>))]
    public async Task<IActionResult> GetStudentGroups(
        [FromQuery] string? orderByExpression,
        [FromQuery] string? searchTerm,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Sender.Send(new GetStudentGroupsWithPagingQuery
        {
            PageSize = pageSize,
            PageNumber = pageNumber,

            SearchTerm = searchTerm ?? string.Empty,

            OrderByExpression = orderByExpression ?? "Name"
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("{studentGroupId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentGroupDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStudentGroup(Guid studentGroupId)
    {
        return Ok(await Sender.Send(new GetStudentGroupQuery(studentGroupId)));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    public async Task<IActionResult> CreateStudentGroup(CreateStudentGroupCommand command)
    {
        var result = await Sender.Send(command);

        return Ok(result);
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

        await Sender.Send(command);

        return NoContent();
    }

    [HttpDelete("{studentGroupId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteStudentGroup(Guid studentGroupId)
    {
        await Sender.Send(new DeleteStudentGroupCommand(studentGroupId));

        return NoContent();
    }
}
