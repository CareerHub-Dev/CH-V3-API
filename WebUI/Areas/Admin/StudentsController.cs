using Application.Common.DTO.Companies;
using Application.Companies.Queries.GetCompanySubscriptionsOfStudent;
using Application.Emails.Commands;
using Application.Experiences.Queries;
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

    [HttpGet("{studentId}/amount-company-subscriptions")]
    public async Task<int> GetAmountCompanySubscriptionsOfStudent(Guid studentId)
    {
        return await Mediator.Send(new GetAmountCompanySubscriptionsOfStudentWithFilterQuery
        {
            StudentId = studentId
        });
    }

    [HttpGet("{studentId}/amount-cvs")]
    public async Task<int> GetAmountCVsOfStudent(Guid studentId)
    {
        return await Mediator.Send(new GetAmountCVsOfStudentWithFilterQuery
        {
            StudentId = studentId
        });
    }

    [HttpGet("{studentId}/amount-jobOffer-subscriptions")]
    public async Task<int> GetAmountJobOfferSubscriptionsOfStudent(Guid studentId)
    {
        return await Mediator.Send(new GetAmountJobOfferSubscriptionsOfStudentWithFilterQuery
        {
            StudentId = studentId
        });
    }

    [HttpGet("{studentId}/amount-student-subscriptions")]
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

    [HttpGet("{studentId}/company-subscriptions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CompanyWithStatsDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCompanySubscriptionsOfStudent(
        Guid studentId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] bool? isCompanyMustBeVerified = null)
    {
        var result = await Mediator.Send(new GetCompanyWithStatsSubscriptionsOfStudentWithPaginationWithSearchWithFilterQuery
        {
            StudentOwnerId = studentId,
            IsStudentOwnerMustBeVerified = true,

            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm,

            IsCompanyMustBeVerified = isCompanyMustBeVerified,
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("{studentId}/Experiences")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ExperienceDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetExperiencesOfStudent(
        Guid studentId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Mediator.Send(new GetExperiencesOfStudentWithPaginationWithFilterQuery
        {
            StudentId = studentId,
            PageNumber = pageNumber,
            PageSize = pageSize,
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
