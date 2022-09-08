using Application.Emails.Commands;
using Application.Students.Commands.DeleteStudent;
using Application.Students.Commands.UpdateStudent;
using Application.Students.Commands.UpdateStudentPhoto;
using Application.Students.Queries.GetAmount;
using Application.Students.Queries.GetStudent;
using Application.Students.Queries.GetStudents;
using Application.Students.Queries.GetStudentSubscriptions;
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
        [FromQuery] GetStudentsWithPaginationWithSearthWithFilterForAdminView view)
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

    [HttpGet("{studentId}")]
    public async Task<StudentDTO> GetStudent(Guid studentId)
    {
        return await Mediator.Send(new GetStudentWithFilterQuery
        {
            StudentId = studentId
        });
    }

    [HttpGet("{studentId}/amountCompanySubscriptions")]
    public async Task<int> GetAmountCompanySubscriptionsOfStudent(Guid studentId)
    {
        return await Mediator.Send(new GetAmountCompanySubscriptionsOfStudentWithFilterQuery
        {
            StudentId = studentId
        });
    }

    [HttpGet("{studentId}/amountCVs")]
    public async Task<int> GetAmountCVsOfStudent(Guid studentId)
    {
        return await Mediator.Send(new GetAmountCVsOfStudentWithFilterQuery
        {
            StudentId = studentId
        });
    }

    [HttpGet("{studentId}/amountJobOfferSubscriptions")]
    public async Task<int> GetAmountJobOfferSubscriptionsOfStudent(Guid studentId)
    {
        return await Mediator.Send(new GetAmountJobOfferSubscriptionsOfStudentWithFilterQuery
        {
            StudentId = studentId
        });
    }

    [HttpGet("{studentId}/amountStudentSubscriptions")]
    public async Task<int> GetAmountStudentSubscriptionsOfStudent(Guid studentId)
    {
        return await Mediator.Send(new GetAmountStudentSubscriptionsOfStudentWithFilterQuery
        {
            StudentId = studentId
        });
    }

    [HttpGet("{studentId}/student-subscriptions")]
    public async Task<IActionResult> GetStudentSubscriptionsOfStudent(
        Guid studentId, 
        [FromQuery] GetStudentsWithPaginationWithSearthWithFilterForAdminView view)
    {
        var result = await Mediator.Send(new GetStudentSubscriptionsOfStudentWithPaginationWithSearchWithFilterQuery
        {
            StudentId = studentId,
            PageNumber = view.PageNumber,
            PageSize = view.PageSize,
            SearchTerm = view.SearchTerm,
            IsVerified = view.IsVerified,
            StudentGroupIds = view.StudentGroupIds,
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
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

    [HttpPut("{studentId}")]
    public async Task<IActionResult> UpdateStudent(Guid studentId, UpdateStudentCommand command)
    {
        if (studentId != command.StudentId)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPost("{studentId}/photo")]
    public async Task<Guid?> UpdateStudentPhoto(Guid studentId, IFormFile? file)
    {
        return await Mediator.Send(new UpdateStudentPhotoCommand { StudentId = studentId, Photo = file });
    }
}
