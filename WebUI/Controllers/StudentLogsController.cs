using Application.Common.Models.Pagination;
using Application.StudentLogs.Commands.CreateStudentLog;
using Application.StudentLogs.Commands.DeleteStudentLog;
using Application.StudentLogs.Commands.UpdateStudentLog;
using Application.StudentLogs.Queries;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebUI.Authorize;
using WebUI.DTO.StudentLog;

namespace WebUI.Controllers;

public class StudentLogsController : ApiControllerBase
{
    /// <summary>
    /// Auth
    /// </summary>
    [HttpGet]
    [Authorize]
    public async Task<IEnumerable<StudentLogResponse>> GetStudentLogs([FromQuery] PaginationParameters paginationParameters)
    {
        var result = await Mediator.Send(new GetStudentLogsQuery
        {
            PaginationParameters = paginationParameters
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return result.Select(x => new StudentLogResponse(x));
    }

    /// <summary>
    /// Admin
    /// </summary>
    [HttpDelete("{studentLogId}")]
    [Authorize("Admin")]
    public async Task<IActionResult> DeleteStudentLog(Guid studentLogId)
    {
        await Mediator.Send(new DeleteStudentLogCommand(studentLogId));
        return NoContent();
    }

    /// <summary>
    /// Admin
    /// </summary>
    [HttpPost]
    [Authorize("Admin")]
    public async Task<ActionResult<Guid>> CreateStudentLog(CreateStudentLogRequest createStudentLog)
    {
        return await Mediator.Send(new CreateStudentLogCommand
        {
            Email = createStudentLog.Email,
            LastName = createStudentLog.LastName,
            FirstName = createStudentLog.FirstName,
            StudentGroupId = createStudentLog.StudentGroupId,
        });
    }

    /// <summary>
    /// Admin
    /// </summary>
    [HttpPut("{studentLogId}")]
    [Authorize("Admin")]
    public async Task<IActionResult> UpdateStudentLog(Guid studentLogId, UpdateStudentLogRequest updateStudentLog)
    {
        await Mediator.Send(new UpdateStudentLogCommand
        {
            Id = studentLogId,
            Email = updateStudentLog.Email,
            LastName = updateStudentLog.LastName,
            FirstName = updateStudentLog.FirstName,
            StudentGroupId = updateStudentLog.StudentGroupId,
        });

        return NoContent();
    }
}
