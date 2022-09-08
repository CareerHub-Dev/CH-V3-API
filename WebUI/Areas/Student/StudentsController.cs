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

    [HttpGet("{studentId}")]
    public async Task<bool> IsStudentOwnerSubscribedToStudentTarget(Guid studentId)
    {
        var result = await Mediator.Send(new IsStudentOwnerSubscribedToStudentTargetWithFilterQuery
        {
            StudentOwnerId = AccountInfo!.Id,
            IsStudentOwnerVerified = true,
            StudentTargetId = studentId,
            IsStudentTargetVerified = true
        });

        return result;
    }

    [HttpPut("{studentId}")]
    public async Task<IActionResult> UpdateOwnStudentAccount(Guid studentId, UpdateStudentCommand command)
    {
        if (studentId != command.StudentId)
        {
            return BadRequest();
        }

        if(studentId != AccountInfo!.Id)
        {
            return StatusCode(403);
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPost("{studentId}/photo")]
    public async Task<ActionResult<Guid?>> UpdateOwnStudentPhoto(Guid studentId, IFormFile? file)
    {
        if (studentId != AccountInfo!.Id)
        {
            return StatusCode(403);
        }

        return await Mediator.Send(new UpdateStudentPhotoCommand { StudentId = studentId, Photo = file });
    }
}
