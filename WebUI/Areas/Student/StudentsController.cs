using Application.Students.Commands.DeleteStudent;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;

namespace WebUI.Areas.Student;

[Authorize("Student")]
[Route("api/Student/[controller]")]
public class StudentsController : ApiControllerBase
{

    /// <remarks> 
    /// Student:
    /// 
    ///     delete own account
    ///
    /// </remarks>
    [HttpDelete("{studentId}")]
    public async Task<IActionResult> DeleteStudent(Guid studentId)
    {
        if (studentId != AccountInfo!.Id)
        {
            return StatusCode(403);
        }

        switch (AccountInfo!.Role)
        {
            case "Admin":
            case "Student" when studentId == AccountInfo.Id:
                await Mediator.Send(new DeleteStudentCommand(studentId));
                break;
            default:
                return StatusCode(403);
        }

        return NoContent();
    }
}
