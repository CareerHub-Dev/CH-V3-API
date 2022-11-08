using API.Authorize;
using Application.Common.DTO.StudentLogs;
using Application.StudentLogs.Commands.CreateStudentLog;
using Application.StudentLogs.Commands.DeleteStudentLog;
using Application.StudentLogs.Commands.UpdateStudentLog;
using Application.StudentLogs.Queries.GetStudentLogs;
using Application.Tags.Queries.GetStudentLog;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Areas.Admin;

[Authorize("Admin", "SuperAdmin")]
[Route("api/Admin/[controller]")]
public class StudentLogsController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<StudentLogDTO>))]
    public async Task<IActionResult> GetStudentLogs(
        [FromQuery] List<Guid>? studentGroupIds,
        [FromQuery] string? orderByExpression,
        [FromQuery] string? searchTerm,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Mediator.Send(new GetStudentLogsWithPaginationWithSearchWithFilterWithSortQuery
        {
            StudentGroupIds = studentGroupIds,
            OrderByExpression = orderByExpression ?? "LastName",
            SearchTerm = searchTerm ?? string.Empty,
            PageNumber = pageNumber,
            PageSize = pageSize
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("{studentLogId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentLogDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStudentLog(Guid studentLogId)
    {
        return Ok(await Mediator.Send(new GetStudentLogQuery(studentLogId)));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateStudentLog(CreateStudentLogCommand command)
    {
        var result = await Mediator.Send(command);

        return Ok(result);
    }

    [HttpPut("{studentLogId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateStudentLog(Guid studentLogId, UpdateStudentLogCommand command)
    {
        if (studentLogId != command.StudentLogId)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{studentLogId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteStudentLog(Guid studentLogId)
    {
        await Mediator.Send(new DeleteStudentLogCommand(studentLogId));

        return NoContent();
    }
}
