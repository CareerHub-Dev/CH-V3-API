using Application.StudentLogs.Commands.CreateStudentLog;
using Application.StudentLogs.Commands.DeleteStudentLog;
using Application.StudentLogs.Commands.UpdateStudentLog;
using Application.StudentLogs.Queries;
using Application.StudentLogs.Queries.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebUI.Authorize;
using WebUI.Common.Models;
using WebUI.Common.Models.StudentLog;

namespace WebUI.Areas.Admin;

[Authorize("Admin")]
[Route("api/Admin/[controller]")]
public class StudentLogsController : ApiControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<StudentLogDTO>> GetStudentLogs([FromQuery] GetStudentLogsWithPaginationWithSearchWithFilterQuery query)
    {
        var result = await Mediator.Send(query);

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return result;
    }

    [HttpDelete("{studentLogId}")]
    public async Task<IActionResult> DeleteStudentLog(Guid studentLogId)
    {
        await Mediator.Send(new DeleteStudentLogCommand(studentLogId));

        return NoContent();
    }

    [HttpPost]
    public async Task<Guid> CreateStudentLog(CreateStudentLogCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{studentLogId}")]
    public async Task<IActionResult> UpdateStudentLog(Guid studentLogId, UpdateStudentLogCommand command)
    {
        if (studentLogId != command.StudentLogId)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }
}
