using Application.Emails.Commands;
using Application.Students.Commands.DeleteStudent;
using Application.Students.Commands.UpdateStudent;
using Application.Students.Commands.UpdateStudentPhoto;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;
using WebUI.Common.Extentions;
using WebUI.Common.Models.Student;

namespace WebUI.Areas.Admin;

[Authorize("Admin", "SuperAdmin")]
[Route("api/Admin/[controller]")]
public class StudentsController : ApiControllerBase
{
    /// <remarks>
    /// Admin:
    /// 
    ///     sends an e-mail
    ///
    /// </remarks>
    [HttpPost("send-verification-email")]
    public async Task<IActionResult> SendVerifyStudentEmail(SendVerifyStudentEmailRequest sendVerifyStudentEmail)
    {
        await Mediator.Send(new SendVerifyStudentEmailCommand(sendVerifyStudentEmail.StudentId));
        return Ok();
    }

    [HttpPut("{studentId}")]
    public async Task<IActionResult> UpdateStudent(Guid studentId, UpdateStudentRequest updateStudent)
    {
        await Mediator.Send(new UpdateStudentCommand
        {
            StudentId = studentId,
            FirstName = updateStudent.FirstName,
            LastName = updateStudent.LastName,
            Phone = updateStudent.Phone,
            StudentGroupId = updateStudent.StudentGroupId,
            BirthDate = updateStudent.BirthDate
        });

        return NoContent();
    }

    [HttpDelete("{studentId}")]
    public async Task<IActionResult> DeleteStudent(Guid studentId)
    {
        await Mediator.Send(new DeleteStudentCommand(studentId));

        return NoContent();
    }

    [HttpPut("{studentId}/photo")]
    public async Task<ActionResult<Guid?>> UpdateStudentPhoto(Guid studentId, [FromForm] UpdateStudentPhotoRequest updateStudentPhoto)
    {
        return await Mediator.Send(new UpdateStudentPhotoCommand
        {
            StudentId = studentId,
            Photo = updateStudentPhoto.PhotoFile is IFormFile photo ? await photo.ToCreateImageAsync() : null,
        });
    }
}
