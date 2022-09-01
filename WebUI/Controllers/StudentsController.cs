using Application.Emails.Commands;
using Application.Students.Commands.DeleteStudent;
using Application.Students.Commands.UpdateStudent;
using Application.Students.Commands.UpdateStudentPhoto;
using Application.Students.Queries;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebUI.Authorize;
using WebUI.Common.Extentions;
using WebUI.Common.Models;
using WebUI.Common.Models.Student;

namespace WebUI.Controllers;

public class StudentsController : ApiControllerBase
{
    /// <summary>
    /// Student
    /// </summary>
    /// <remarks>
    /// Student
    /// 
    ///     get all Verified students
    ///
    /// </remarks>
    [HttpGet]
    [Authorize("Student")]
    public async Task<IActionResult> GetStudents(
        [FromQuery] PaginationParameters paginationParameters, 
        [FromQuery] SearchParameter searchParameter)
    {
        var result = await Mediator.Send(new GetStudentBriefsWithPaginationWithSearthWithFilterQuery
        {
            PageNumber = paginationParameters.PageNumber,
            PageSize = paginationParameters.PageSize,
            SearchTerm = searchParameter.SearchTerm,
            WithoutStudentId = AccountInfo!.Id,
            IsVerified = true
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result.Select(x => new StudentBriefResponse(x)));
    }

    /// <summary>
    /// Admin
    /// </summary>
    /// <remarks>
    /// Admin:
    /// 
    ///     sends an e-mail
    ///
    /// </remarks>
    [HttpPost("send-verification-email")]
    [Authorize("Admin")]
    public async Task<IActionResult> SendVerifyStudentEmail(SendVerifyStudentEmailRequest sendVerifyStudentEmail)
    {
        await Mediator.Send(new SendVerifyStudentEmailCommand(sendVerifyStudentEmail.StudentId));
        return Ok();
    }


    /// <summary>
    /// Admin Student
    /// </summary>
    /// <remarks>
    /// Admin:
    /// 
    ///     update any student
    ///     
    /// Student:
    /// 
    ///     update own account
    ///
    /// </remarks>
    [HttpPut("{studentId}")]
    [Authorize("Admin", "Student")]
    public async Task<IActionResult> UpdateStudent(Guid studentId, UpdateStudentRequest updateStudent)
    {
        switch (AccountInfo!.Role)
        {
            case "Admin":
            case "Student" when studentId == AccountInfo.Id:
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
                    break;
                }
            default:
                return StatusCode(403);
        }

        return NoContent();
    }

    /// <summary>
    /// Admin Student
    /// </summary>
    /// <remarks>
    /// Admin:
    /// 
    ///     delete any student
    ///     
    /// Student:
    /// 
    ///     delete own account
    ///
    /// </remarks>
    [HttpDelete("{studentId}")]
    [Authorize("Admin", "Student")]
    public async Task<IActionResult> DeleteStudent(Guid studentId)
    {
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

    /// <summary>
    /// Admin Student
    /// </summary>
    /// <remarks>
    /// Admin:
    /// 
    ///     update any student photo
    ///     
    /// Student:
    /// 
    ///     update own photo
    ///
    /// </remarks>
    [HttpPut("{studentId}/photo")]
    [Authorize("Admin", "Student")]
    public async Task<ActionResult<Guid?>> UpdateStudentPhoto(Guid studentId, [FromForm] UpdateStudentPhotoRequest updateStudentPhoto)
    {
        switch (AccountInfo!.Role)
        {
            case "Admin":
            case "Student" when studentId == AccountInfo.Id:
                {
                    return await Mediator.Send(new UpdateStudentPhotoCommand
                    {
                        StudentId = studentId,
                        Photo = updateStudentPhoto.PhotoFile is IFormFile photo ? await photo.ToCreateImageAsync() : null,
                    });
                }
            default:
                return StatusCode(403);
        }
    }
}
