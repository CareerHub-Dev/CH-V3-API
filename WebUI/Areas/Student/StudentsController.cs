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

namespace WebUI.Areas.Student;

[Authorize("Student")]
[Route("api/Student/[controller]")]
public class StudentsController : ApiControllerBase
{
    /// <remarks>
    /// Student
    /// 
    ///     get all Verified Brief students
    ///     
    /// </remarks>
    [HttpGet]
    public async Task<IEnumerable<StudentBriefResponse>> GetStudents(
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

        return result.Select(x => new StudentBriefResponse(x));
    }

    /// <remarks>
    /// Student
    /// 
    ///     get Verified Detailed student
    ///     
    /// </remarks>
    [HttpGet("{studentId}")]
    public async Task<StudentDetailedResponse> GetStudent(Guid studentId)
    {
        var result = await Mediator.Send(new GetStudentDetailedWithFilterQuery
        {
            StudentId = studentId,
            IsVerified = true
        });

        return new StudentDetailedResponse(result);
    }


    /// <remarks>  
    /// Student:
    /// 
    ///     update own account
    ///
    /// </remarks>
    [HttpPut("{studentId}")]
    public async Task<IActionResult> UpdateStudent(Guid studentId, UpdateStudentRequest updateStudent)
    {
        if (studentId != AccountInfo!.Id)
        {
            return StatusCode(403);
        }

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

    /// <remarks> 
    /// Student:
    /// 
    ///     update own photo
    ///
    /// </remarks>
    [HttpPut("{studentId}/photo")]
    public async Task<ActionResult<Guid?>> UpdateStudentPhoto(Guid studentId, [FromForm] UpdateStudentPhotoRequest updateStudentPhoto)
    {
        if (studentId != AccountInfo!.Id)
        {
            return StatusCode(403);
        }

        return await Mediator.Send(new UpdateStudentPhotoCommand
        {
            StudentId = studentId,
            Photo = updateStudentPhoto.PhotoFile is IFormFile photo ? await photo.ToCreateImageAsync() : null,
        });
    }
}
