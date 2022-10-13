using Application.Common.DTO.Companies;
using Application.Common.DTO.Experiences;
using Application.Common.DTO.Students;
using Application.Companies.Queries.GetCompanySubscriptionsOfStudent;
using Application.Emails.Commands;
using Application.Experiences.Queries.GetExperiences;
using Application.Students.Commands.DeleteStudent;
using Application.Students.Commands.UpdateStudentDetail;
using Application.Students.Commands.UpdateStudentPhoto;
using Application.Students.Queries.GetAmount;
using Application.Students.Queries.GetStudent;
using Application.Students.Queries.GetStudents;
using Application.Students.Queries.GetStudentSubscriptionsOfStudent;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using API.Authorize;
using Application.Common.DTO.JobOffers;
using Application.JobOffers.Queries.GetJobOfferSubscriptionsOfStudent;

using JobOfferStatsFilter = Application.JobOffers.Queries.Models.StatsFilter;

namespace API.Areas.Admin;

[Authorize("Admin", "SuperAdmin")]
[Route("api/Admin/[controller]")]
public class StudentsController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<StudentDTO>))]
    public async Task<IActionResult> GetStudents(
        [FromQuery] ActivationStatus? studentMustHaveActivationStatus,
        [FromQuery] bool? isStudentMustBeVerified,
        [FromQuery] List<Guid>? studentGroupIds,
        [FromQuery] string? orderByExpression,
        [FromQuery] string? searchTerm,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Mediator.Send(new GetStudentsWithPaginationWithSearchWithFilterWithSortQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm ?? string.Empty,
            IsStudentMustBeVerified = isStudentMustBeVerified,
            StudentGroupIds = studentGroupIds,
            StudentMustHaveActivationStatus = studentMustHaveActivationStatus,
            OrderByExpression = orderByExpression ?? "LastName",
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("{studentId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStudent(Guid studentId)
    {
        return Ok(await Mediator.Send(new GetStudentWithFilterQuery
        {
            StudentId = studentId
        }));
    }

    [HttpGet("{studentId}/amount-company-subscriptions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountCompanySubscriptionsOfStudent(Guid studentId)
    {
        return Ok(await Mediator.Send(new GetAmountCompanySubscriptionsOfStudentWithFilterQuery
        {
            StudentId = studentId
        }));
    }

    [HttpGet("{studentId}/amount-cvs")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountCVsOfStudent(Guid studentId)
    {
        return Ok(await Mediator.Send(new GetAmountCVsOfStudentWithFilterQuery
        {
            StudentId = studentId
        }));
    }

    [HttpGet("{studentId}/amount-jobOffer-subscriptions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountJobOfferSubscriptionsOfStudent(Guid studentId)
    {
        return Ok(await Mediator.Send(new GetAmountJobOfferSubscriptionsOfStudentWithFilterQuery
        {
            StudentId = studentId
        }));
    }

    [HttpGet("{studentId}/amount-student-subscriptions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountStudentSubscriptionsOfStudent(Guid studentId)
    {
        return Ok(await Mediator.Send(new GetAmountStudentSubscriptionsOfStudentWithFilterQuery
        {
            StudentId = studentId
        }));
    }

    [HttpGet("{studentId}/student-subscriptions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<StudentDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStudentSubscriptionsOfStudent(
        Guid studentId,
        [FromQuery] ActivationStatus? studentMustHaveActivationStatus,
        [FromQuery] bool? isStudentMustBeVerified,
        [FromQuery] List<Guid>? studentGroupIds,
        [FromQuery] string? orderByExpression,
        [FromQuery] string? searchTerm,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Mediator.Send(new GetStudentSubscriptionsOfStudentWithPaginationWithSearchWithFilterWithSortQuery
        {
            StudentOwnerId = studentId,

            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm ?? string.Empty,

            IsStudentMustBeVerified = isStudentMustBeVerified,
            StudentGroupIds = studentGroupIds,
            StudentMustHaveActivationStatus = studentMustHaveActivationStatus,

            OrderByExpression = orderByExpression ?? "LastName",
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("{studentId}/company-subscriptions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CompanyWithStatsDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCompanySubscriptionsOfStudent(
        Guid studentId,
        [FromQuery] ActivationStatus? companyMustHaveActivationStatus,
        [FromQuery] bool? isCompanyMustBeVerified,
        [FromQuery] string? orderByExpression,
        [FromQuery] string? searchTerm,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Mediator.Send(new GetCompanyWithStatsSubscriptionsOfStudentWithPaginationWithSearchWithFilterWithSortQuery
        {
            StudentOwnerId = studentId,

            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm ?? string.Empty,

            IsCompanyMustBeVerified = isCompanyMustBeVerified,
            CompanyMustHaveActivationStatus = companyMustHaveActivationStatus,
            OrderByExpression = orderByExpression ?? "Name",
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("{studentId}/jobOffer-subscriptions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<DetiledJobOfferWithStatsDTO>))]
    public async Task<IActionResult> GetJobOfferSubscriptionsOfStudent(
        Guid studentId,
        [FromQuery] string? orderByExpression,
        [FromQuery] string? searchTerm,
        [FromQuery] bool? isJobOfferMustBeActive,
        [FromQuery] JobType? mustHaveJobType,
        [FromQuery] WorkFormat? mustHaveWorkFormat,
        [FromQuery] ExperienceLevel? mustHaveExperienceLevel,
        [FromQuery] Guid? mustHavejobPositionId,
        [FromQuery] List<Guid>? mustHaveTagIds,
        [FromQuery] bool? isCompanyOfJobOfferMustBeVerified,
        [FromQuery] ActivationStatus companyOfJobOfferMustHaveActivationStatus,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Mediator.Send(new GetDetiledJobOfferSubscriptionsWithStatsOfStudentOwnerWithPaginationWithSearchWithFilterWithSortQuery
        {
            StudentOwnerId = studentId,

            PageNumber = pageNumber,
            PageSize = pageSize,

            SearchTerm = searchTerm ?? string.Empty,

            IsJobOfferMustBeActive = isJobOfferMustBeActive,
            MustHaveWorkFormat = mustHaveWorkFormat,
            MustHaveJobType = mustHaveJobType,
            MustHaveExperienceLevel = mustHaveExperienceLevel,
            MustHaveJobPositionId = mustHavejobPositionId,
            MustHaveTagIds = mustHaveTagIds,
            IsCompanyOfJobOfferMustBeVerified = isCompanyOfJobOfferMustBeVerified,
            CompanyOfJobOfferMustHaveActivationStatus = companyOfJobOfferMustHaveActivationStatus,

            OrderByExpression = orderByExpression ?? "StartDate",
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("{studentId}/Experiences")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ExperienceDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetExperiencesOfStudent(
        Guid studentId,
        [FromQuery] string? orderByExpression,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Mediator.Send(new GetExperiencesOfStudentWithPaginationWithFilterWithSortQuery
        {
            StudentId = studentId,

            PageNumber = pageNumber,
            PageSize = pageSize,

            OrderByExpression = orderByExpression ?? "Title"
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
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SendVerifyStudentEmail(SendVerifyStudentEmailCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }

    [HttpPut("{studentId}/detail")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateStudentDetail(Guid studentId, UpdateStudentDetailCommand command)
    {
        if (studentId != command.StudentId)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPost("{studentId}/photo")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid?))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStudentPhoto(Guid studentId, IFormFile? file)
    {
        return Ok(await Mediator.Send(new UpdateStudentPhotoCommand { StudentId = studentId, Photo = file }));
    }

    [HttpDelete("{studentId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteStudent(Guid studentId)
    {
        await Mediator.Send(new DeleteStudentCommand(studentId));

        return NoContent();
    }
}
