using Application.StudentLogs.Commands.CreateStudentLog;
using Application.StudentLogs.Commands.DeleteStudentLog;
using Application.StudentLogs.Commands.UpdateStudentLog;
using Application.StudentLogs.Queries;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebUI.Authorize;
using WebUI.Common.Models;
using WebUI.Common.Models.StudentLog;

namespace WebUI.Controllers;

[Authorize("Admin")]
public class StudentLogsController : ApiControllerBase
{
    /// <summary>
    /// Auth
    /// </summary>
    [HttpGet]
    public async Task<IEnumerable<StudentLogResponse>> GetStudentLogs(
        [FromQuery] PaginationParameters paginationParameters,
        [FromQuery] SearchParameter searchParameter,
        [FromQuery] StudentLogListFilterParameters filterParameters)
    {
        var result = await Mediator.Send(new GetStudentLogsWithPaginationWithSearchWithFilterQuery
        {
            PageSize = paginationParameters.PageSize,
            PageNumber = paginationParameters.PageNumber,
            SearchTerm = searchParameter.SearchTerm,
            StudentGroupId = filterParameters.StudentGroupId
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return result.Select(x => new StudentLogResponse(x));
    }

    /// <summary>
    /// Admin
    /// </summary>
    [HttpGet("{studentLogId}")]
    public async Task<StudentLogResponse> GetStudentLog(Guid studentLogId)
    {
        var result = await Mediator.Send(new GetStudentLogQuery(studentLogId));
        return new StudentLogResponse(result);
    }

    /// <summary>
    /// Admin
    /// </summary>
    [HttpDelete("{studentLogId}")]
    public async Task<IActionResult> DeleteStudentLog(Guid studentLogId)
    {
        await Mediator.Send(new DeleteStudentLogCommand(studentLogId));
        return NoContent();
    }

    /// <summary>
    /// Admin
    /// </summary>
    [HttpPost]
    public async Task<Guid> CreateStudentLog(CreateStudentLogRequest createStudentLog)
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
    public async Task<IActionResult> UpdateStudentLog(Guid studentLogId, UpdateStudentLogRequest updateStudentLog)
    {
        await Mediator.Send(new UpdateStudentLogCommand
        {
            StudentLogId = studentLogId,
            Email = updateStudentLog.Email,
            LastName = updateStudentLog.LastName,
            FirstName = updateStudentLog.FirstName,
            StudentGroupId = updateStudentLog.StudentGroupId,
        });

        return NoContent();
    }
}
