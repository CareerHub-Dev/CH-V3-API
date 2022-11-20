using API.Authorize;
using Application.Common.DTO.Companies;
using Application.Common.DTO.Experiences;
using Application.Common.DTO.JobOffers;
using Application.Common.DTO.Students;
using Application.Common.Enums;
using Application.Companies.Queries.GetBriefCompanySubscriptionsWithStatsOfStudentWithPaging;
using Application.Emails.Commands.SendVerifyStudentEmail;
using Application.Experiences.Queries.GetExperiencesOfStudentWithPaging;
using Application.JobOffers.Queries.GetDetiledJobOfferSubscriptionsWithStatsOfStudentWithPaging;
using Application.Students.Commands.DeleteStudent;
using Application.Students.Commands.UpdateStudentDetail;
using Application.Students.Commands.UpdateStudentPhoto;
using Application.Students.Queries.GetAmountCompanySubscriptionsOfStudent;
using Application.Students.Queries.GetAmountCVsOfStudent;
using Application.Students.Queries.GetAmountJobOfferSubscriptionsOfStudent;
using Application.Students.Queries.GetAmountStudentSubscribersOfStudent;
using Application.Students.Queries.GetAmountStudentSubscriptionsOfStudent;
using Application.Students.Queries.GetStudent;
using Application.Students.Queries.GetStudents;
using Application.Students.Queries.GetStudentSubscribersOfStudentWithPaging;
using Application.Students.Queries.GetStudentSubscriptionsOfStudentWithPaging;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Areas.Admin;

[Authorize(Role.Admin, Role.SuperAdmin)]
[Route("api/Admin/[controller]")]
public class StudentsController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<StudentDTO>))]
    public async Task<IActionResult> GetStudents(
        [FromQuery] bool? isStudentMustBeVerified,
        [FromQuery] List<Guid>? studentGroupIds,
        [FromQuery] string? orderByExpression,
        [FromQuery] string? searchTerm,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Sender.Send(new GetStudentsWithPagingQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm ?? string.Empty,
            IsStudentMustBeVerified = isStudentMustBeVerified,
            StudentGroupIds = studentGroupIds,
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
        return Ok(await Sender.Send(new GetStudentQuery
        {
            StudentId = studentId
        }));
    }

    [HttpGet("{studentId}/amount-company-subscriptions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountCompanySubscriptionsOfStudent(Guid studentId)
    {
        return Ok(await Sender.Send(new GetAmountCompanySubscriptionsOfStudentQuery
        {
            StudentId = studentId
        }));
    }

    [HttpGet("{studentId}/amount-cvs")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountCVsOfStudent(Guid studentId)
    {
        return Ok(await Sender.Send(new GetAmountCVsOfStudentQuery
        {
            StudentId = studentId
        }));
    }

    [HttpGet("{studentId}/amount-jobOffer-subscriptions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountJobOfferSubscriptionsOfStudent(Guid studentId)
    {
        return Ok(await Sender.Send(new GetAmountJobOfferSubscriptionsOfStudentQuery
        {
            StudentId = studentId
        }));
    }

    [HttpGet("{studentId}/amount-student-subscriptions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountStudentSubscriptionsOfStudent(Guid studentId)
    {
        return Ok(await Sender.Send(new GetAmountStudentSubscriptionsOfStudentQuery
        {
            StudentId = studentId
        }));
    }

    [HttpGet("{studentId}/amount-student-subscribers")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountStudentSubscribersOfStudent(Guid studentId)
    {
        return Ok(await Sender.Send(new GetAmountStudentSubscribersOfStudentQuery
        {
            StudentId = studentId
        }));
    }

    [HttpGet("{studentId}/student-subscriptions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<StudentDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStudentSubscriptionsOfStudent(
        Guid studentId,
        [FromQuery] bool? isStudentMustBeVerified,
        [FromQuery] List<Guid>? studentGroupIds,
        [FromQuery] string? orderByExpression,
        [FromQuery] string? searchTerm,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Sender.Send(new GetStudentSubscriptionsOfStudentWithPagingQuery
        {
            StudentId = studentId,

            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm ?? string.Empty,

            IsStudentSubscriptionMustBeVerified = isStudentMustBeVerified,
            StudentGroupIds = studentGroupIds,

            OrderByExpression = orderByExpression ?? "LastName",
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("{studentId}/student-subscribers")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<StudentDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStudentSubscribersOfStudent(
        Guid studentId,
        [FromQuery] bool? isStudentMustBeVerified,
        [FromQuery] List<Guid>? studentGroupIds,
        [FromQuery] string? orderByExpression,
        [FromQuery] string? searchTerm,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Sender.Send(new GetStudentSubscribersOfStudentWithPagingQuery
        {
            StudentId = studentId,

            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm ?? string.Empty,

            IsStudentSubscriberMustBeVerified = isStudentMustBeVerified,
            StudentGroupIds = studentGroupIds,

            OrderByExpression = orderByExpression ?? "LastName",
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("{studentId}/company-subscriptions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BriefCompanyWithStatsDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCompanySubscriptionsOfStudent(
        Guid studentId,
        [FromQuery] bool? isCompanyMustBeVerified,
        [FromQuery] string? orderByExpression,
        [FromQuery] string? searchTerm,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Sender.Send(new GetBriefCompanySubscriptionsWithStatsOfStudentWithPagingQuery
        {
            StudentId = studentId,

            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm ?? string.Empty,

            IsCompanyMustBeVerified = isCompanyMustBeVerified,
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
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Sender.Send(new GetDetiledJobOfferSubscriptionsWithStatsOfStudentWithPagingQuery
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
        var result = await Sender.Send(new GetExperiencesOfStudentWithPagingQuery
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
        await Sender.Send(command);

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

        await Sender.Send(command);

        return NoContent();
    }

    [HttpPost("{studentId}/photo")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStudentPhoto(Guid studentId, IFormFile? file)
    {
        return Ok(await Sender.Send(new UpdateStudentPhotoCommand { StudentId = studentId, Photo = file }));
    }

    [HttpDelete("{studentId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteStudent(Guid studentId)
    {
        await Sender.Send(new DeleteStudentCommand(studentId));

        return NoContent();
    }
}
