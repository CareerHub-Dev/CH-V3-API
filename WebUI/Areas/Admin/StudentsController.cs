using Application.Emails.Commands;
using Application.Students.Commands.DeleteStudent;
using Application.Students.Queries;
using Application.Students.Queries.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebUI.Authorize;
using WebUI.Common.Models.Student;

namespace WebUI.Areas.Admin;

[Authorize("Admin", "SuperAdmin")]
[Route("api/Admin/[controller]")]
public class StudentsController : ApiControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<StudentDTO>> GetStudents(
        [FromQuery] GetStudentsWithPaginationWithSearthWithFilterView view)
    {
        var result = await Mediator.Send(new GetStudentsWithPaginationWithSearthWithFilterQuery
        {
            PageNumber = view.PageNumber,
            PageSize = view.PageSize,
            SearchTerm = view.SearchTerm,
            IsVerified = view.IsVerified,
            StudentGroupIds = view.StudentGroupIds,
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return result;
    }

    /// <remarks>
    /// Admin:
    /// 
    ///     sends an e-mail
    ///
    /// </remarks>
    [HttpPost("send-verification-email")]
    public async Task<IActionResult> SendVerifyStudentEmail(SendVerifyStudentEmailCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }

    [HttpDelete("{studentId}")]
    public async Task<IActionResult> DeleteStudent(Guid studentId)
    {
        await Mediator.Send(new DeleteStudentCommand(studentId));

        return NoContent();
    }
}
