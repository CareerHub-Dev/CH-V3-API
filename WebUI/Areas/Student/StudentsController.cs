using Application.Companies.Queries.GetCompanySubscriptionsOfStudent;
using Application.Companies.Queries.Models;
using Application.Students.Commands.UpdateStudent;
using Application.Students.Commands.UpdateStudentPhoto;
using Application.Students.Queries;
using Application.Students.Queries.GetAmount;
using Application.Students.Queries.GetStudent;
using Application.Students.Queries.GetStudents;
using Application.Students.Queries.GetStudentSubscriptions;
using Application.Students.Queries.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebUI.Authorize;
using WebUI.Common.Models.Student;

namespace WebUI.Areas.Student;

[Authorize("Student")]
[Route("api/Student/[controller]")]
public class StudentsController : ApiControllerBase
{

    [HttpGet]
    public async Task<IEnumerable<FollowedStudentDetailedDTO>> GetStudents(
        [FromQuery] GetStudentsWithPaginationWithSearthWithFilterForStudentView view)
    {
        var result = await Mediator.Send(new GetFollowedStudentDetailedsForFollowerStudentWithPaginationWithSearchWithFilterQuery
        {
            FollowerStudentId = AccountInfo!.Id,
            IsFollowerStudentVerified = true,

            PageNumber = view.PageNumber,
            PageSize = view.PageSize,
            SearchTerm = view.SearchTerm,

            IsVerified = true,
            WithoutStudentId = AccountInfo!.Id,
            StudentGroupIds = view.StudentGroupIds,
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return result;
    }

    [HttpGet("{studentId}")]
    public async Task<StudentDetailedDTO> GetStudent(Guid studentId)
    {
        return await Mediator.Send(new GetStudentDetailedWithQuery
        {
            StudentId = studentId,
            IsVerified = true
        });
    }

    [HttpGet("{studentId}/amountCompanySubscriptions")]
    public async Task<int> GetAmountCompanySubscriptionsOfStudent(Guid studentId)
    {
        return await Mediator.Send(new GetAmountCompanySubscriptionsOfStudentWithFilterQuery
        {
            StudentId = studentId,
            IsVerified = true,
            IsCompanyVerified = true
        });
    }

    [HttpGet("{studentId}/amountCVs")]
    public async Task<int> GetAmountCVsOfStudent(Guid studentId)
    {
        return await Mediator.Send(new GetAmountCVsOfStudentWithFilterQuery
        {
            StudentId = studentId,
            IsVerified = true,

        });
    }

    [HttpGet("{studentId}/amountJobOfferSubscriptions")]
    public async Task<int> GetAmountJobOfferSubscriptionsOfStudent(Guid studentId)
    {
        return await Mediator.Send(new GetAmountJobOfferSubscriptionsOfStudentWithFilterQuery
        {
            StudentId = studentId,
            IsVerified = true,
            IsJobOfferActive = true
        });
    }

    [HttpGet("{studentId}/amountStudentSubscriptions")]
    public async Task<int> GetAmountStudentSubscriptionsOfStudent(Guid studentId)
    {
        return await Mediator.Send(new GetAmountStudentSubscriptionsOfStudentWithFilterQuery
        {
            StudentId = studentId,
            IsVerified = true,
            IsStudentTargetOfSubscriptionVerified = true
        });
    }

    [HttpGet("{studentId}/student-subscriptions")]
    public async Task<IActionResult> GetStudentSubscriptionsOfStudent(
        Guid studentId,
        [FromQuery] GetStudentsWithPaginationWithSearthWithFilterForAdminView view)
    {
        var result = await Mediator.Send(new GetFollowedStudentDetailedSubsciptionsOfStudentOwnerForFollowerStudentWithPaginationWithSearchWithFilterQuery
        {
            FollowerStudentId = AccountInfo!.Id,
            IsFollowerStudentVerified = true,

            StudentOwnerId = studentId,
            IsStudentOwnerVerified = true,

            PageNumber = view.PageNumber,
            PageSize = view.PageSize,
            SearchTerm = view.SearchTerm,

            IsVerified = view.IsVerified,
            WithoutStudentId = AccountInfo!.Id,
            StudentGroupIds = view.StudentGroupIds,
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("{studentId}/company-subscriptions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FollowedDetailedCompanyWithStatsDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCompanySubscriptionsOfStudent(
        Guid studentId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null)
    {
        var result = await Mediator.Send(new GetFollowedDetailedCompanyWithStatsSubscriptionsOfStudentForFollowerStudentWithPaginationWithSearchWithFilterQuery
        {
            FollowerStudentId = AccountInfo!.Id,
            IsFollowerStudentMustBeVerified = true,

            StudentOwnerId = studentId,
            IsStudentOwnerMustBeVerified = true,

            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm,

            IsCompanyMustBeVerified = true,

            StatsFilter = new StatsFilter
            {
                IsJobOfferMustBeActive = true,
                IsSubscriberMustBeVerified = true,
            }
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("{studentTargetId}/subscribe")]
    public async Task<bool> IsStudentOwnerSubscribedToStudentTarget(Guid studentTargetId)
    {
        var result = await Mediator.Send(new IsStudentOwnerSubscribedToStudentTargetWithFilterQuery
        {
            StudentOwnerId = AccountInfo!.Id,
            IsStudentOwnerVerified = true,
            StudentTargetId = studentTargetId,
            IsStudentTargetVerified = true
        });

        return result;
    }

    [HttpPut("own")]
    public async Task<IActionResult> UpdateOwnStudentAccount(UpdateOwnStudentAccountView view)
    {
        await Mediator.Send(new UpdateStudentCommand
        {
            StudentId = AccountInfo!.Id,
            FirstName = view.FirstName,
            LastName = view.LastName,
            Phone = view.Phone,
            BirthDate = view.BirthDate,
            StudentGroupId = view.StudentGroupId,
        });

        return NoContent();
    }

    [HttpPost("own/photo")]
    public async Task<ActionResult<Guid?>> UpdateOwnStudentPhoto(IFormFile? file)
    {
        return await Mediator.Send(new UpdateStudentPhotoCommand { StudentId = AccountInfo!.Id, Photo = file });
    }
}
