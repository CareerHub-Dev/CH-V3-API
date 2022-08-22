using Application.Common.Models.Pagination;
using Application.Common.Models.Search;
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
    public async Task<ActionResult<IEnumerable<StudentLogResponse>>> GetStudentLogs(
        [FromQuery] PaginationParameters paginationParameters, 
        [FromQuery] SearchParameter searchParameter, 
        [FromQuery] StudentLogListFilterParameters filterParameters)
    {
        var result = await Mediator.Send(new GetStudentLogsWithPaginationWithSearchWithFilterQuery
        {
            PaginationParameters = paginationParameters,
            SearchTerm = searchParameter.SearchTerm,
            CreatedBy = filterParameters.CreatedBy,
            LastModifiedBy = filterParameters.LastModifiedBy,
            StudentGroupId = filterParameters.StudentGroupId
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result.Select(x => new StudentLogResponse(x)));
    }

    /// <summary>
    /// Admin
    /// </summary>
    [HttpGet("{studentLogId}")]
    [Authorize("Admin")]
    public async Task<ActionResult<StudentLogResponse>> GetStudentLog(Guid studentLogId)
    {
        var result = await Mediator.Send(new GetStudentLogQuery(studentLogId));
        return new StudentLogResponse(result);
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
    public async Task<ActionResult<Guid>> CreateStudentLog(CreateStudentLogRequest request)
    {
        return await Mediator.Send(new CreateStudentLogCommand
        {
            Email = request.Email,
            LastName = request.LastName,
            FirstName = request.FirstName,
            StudentGroupId = request.StudentGroupId,
        });
    }

    /// <summary>
    /// Admin
    /// </summary>
    [HttpPut("{studentLogId}")]
    [Authorize("Admin")]
    public async Task<IActionResult> UpdateStudentLog(Guid studentLogId, UpdateStudentLogRequest request)
    {
        await Mediator.Send(new UpdateStudentLogCommand
        {
            StudentLogId = studentLogId,
            Email = request.Email,
            LastName = request.LastName,
            FirstName = request.FirstName,
            StudentGroupId = request.StudentGroupId,
        });

        return NoContent();
    }
}
