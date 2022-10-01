using Application.Common.DTO.Companies;
using Application.Companies.Queries.GetCompanySubscriptionsOfStudent;
using Application.Companies.Queries.Models;
using Application.Experiences.Queries;
using Application.Students.Commands.DeleteStudent;
using Application.Students.Commands.UpdateStudentDetail;
using Application.Students.Commands.UpdateStudentPhoto;
using Application.Students.Commands.VerifiedActiveStudentOwnerSubscribeToVerifiedActiveStudentTarget;
using Application.Students.Commands.VerifiedActiveStudentOwnerUnsubscribeFromVerifiedActiveStudentTarget;
using Application.Students.Queries;
using Application.Students.Queries.GetAmount;
using Application.Students.Queries.GetStudent;
using Application.Students.Queries.GetStudents;
using Application.Students.Queries.GetStudentSubscriptions;
using Application.Students.Queries.Models;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebUI.Authorize;
using WebUI.Common.Models.Student;
using WebUI.ViewModels.Students;

namespace WebUI.Areas.Student;

[Authorize("Student")]
[Route("api/Student/[controller]")]
public class StudentsController : ApiControllerBase
{
    #region GetStudents 

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

    #endregion

    #region GetStudentStatistic

    [HttpGet("{studentId}/amount-company-subscriptions")]
    public async Task<int> GetAmountCompanySubscriptionsOfStudent(Guid studentId)
    {
        return await Mediator.Send(new GetAmountCompanySubscriptionsOfStudentWithFilterQuery
        {
            StudentId = studentId,
            IsStudentMustBeVerified = true,
            IsCompanyMustBeVerified = true
        });
    }

    [HttpGet("{studentId}/amount-jobOffer-subscriptions")]
    public async Task<int> GetAmountJobOfferSubscriptionsOfStudent(Guid studentId)
    {
        return await Mediator.Send(new GetAmountJobOfferSubscriptionsOfStudentWithFilterQuery
        {
            StudentId = studentId,
            IsStudentMustBeVerified = true,
            IsJobOfferMustBeActive = true
        });
    }

    [HttpGet("{studentId}/amount-student-subscriptions")]
    public async Task<int> GetAmountStudentSubscriptionsOfStudent(Guid studentId)
    {
        return await Mediator.Send(new GetAmountStudentSubscriptionsOfStudentWithFilterQuery
        {
            StudentId = studentId,
            IsStudentMustBeVerified = true,
            IsStudentTargetOfSubscriptionMustBeVerified = true
        });
    }

    #endregion

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
        [FromQuery] string orderByExpression = "Name",
        [FromQuery] string searchTerm = "")
    {
        var result = await Mediator.Send(new GetFollowedDetailedCompanyWithStatsSubscriptionsOfStudentForFollowerStudentWithPaginationWithSearchWithFilterWithSortQuery
        {
            FollowerStudentId = AccountInfo!.Id,
            IsFollowerStudentMustBeVerified = true,
            FollowerStudentMustHaveActivationStatus = ActivationStatus.Active,

            StudentOwnerId = studentId,
            IsStudentOwnerMustBeVerified = true,
            StudentOwnerMustHaveActivationStatus = ActivationStatus.Active,

            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm,

            IsCompanyMustBeVerified = true,
            CompanyMustHaveActivationStatus = ActivationStatus.Active,

            StatsFilter = new StatsFilter
            {
                IsJobOfferMustBeActive = true,

                IsSubscriberMustBeVerified = true,
                SubscriberMustHaveActivationStatus = ActivationStatus.Active
            },

            OrderByExpression = orderByExpression,
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    #region GetExperiencesOfStudent

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
            IsStudentMustBeVerified = true,
            PageNumber = pageNumber,
            PageSize = pageSize,
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    #endregion

    #region GetSelfStudent

    [HttpGet("self")]
    public async Task<StudentDetailedDTO> GetSelfStudent()
    {
        return await Mediator.Send(new GetStudentDetailedWithQuery
        {
            StudentId = AccountInfo!.Id,
            IsVerified = true
        });
    }

    #endregion

    #region SelfSubscribe

    [HttpGet("{studentTargetId}/subscribe")]
    public async Task<bool> IsStudentOwnerSubscribedToStudentTarget(Guid studentTargetId)
    {
        var result = await Mediator.Send(new IsVerifiedStudentOwnerSubscribedToVerifiedStudentTargetWithFilterQuery
        {
            StudentOwnerId = AccountInfo!.Id,
            StudentTargetId = studentTargetId,
        });

        return result;
    }

    #endregion

    #region GetSelfStatistic

    [HttpGet("self/amount-company-subscriptions")]
    public async Task<int> GetAmountCompanySubscriptionsOfSelfStudent()
    {
        return await Mediator.Send(new GetAmountCompanySubscriptionsOfStudentWithFilterQuery
        {
            StudentId = AccountInfo!.Id,
            IsStudentMustBeVerified = true,
            IsCompanyMustBeVerified = true
        });
    }

    [HttpGet("self/amount-jobOffer-subscriptions")]
    public async Task<int> GetAmountJobOfferSubscriptionsOfSelfStudent()
    {
        return await Mediator.Send(new GetAmountJobOfferSubscriptionsOfStudentWithFilterQuery
        {
            StudentId = AccountInfo!.Id,
            IsStudentMustBeVerified = true,
            IsJobOfferMustBeActive = true
        });
    }

    [HttpGet("self/amount-student-subscriptions")]
    public async Task<int> GetAmountStudentSubscriptionsOfSelfStudent()
    {
        return await Mediator.Send(new GetAmountStudentSubscriptionsOfStudentWithFilterQuery
        {
            StudentId = AccountInfo!.Id,
            IsStudentMustBeVerified = true,
            IsStudentTargetOfSubscriptionMustBeVerified = true
        });
    }

    [HttpGet("self/amount-cvs")]
    public async Task<int> GetAmountCVsOfSelfStudent()
    {
        return await Mediator.Send(new GetAmountCVsOfStudentWithFilterQuery
        {
            StudentId = AccountInfo!.Id,
            IsStudentMustBeVerified = true,
        });
    }

    #endregion

    [HttpGet("self/student-subscriptions")]
    public async Task<IActionResult> GetStudentSubscriptionsOfSelfStudent(
        [FromQuery] GetStudentsWithPaginationWithSearthWithFilterForAdminView view)
    {
        var result = await Mediator.Send(new GetFollowedStudentDetailedSubsciptionsOfStudentOwnerForFollowerStudentWithPaginationWithSearchWithFilterQuery
        {
            FollowerStudentId = AccountInfo!.Id,
            IsFollowerStudentVerified = true,

            StudentOwnerId = AccountInfo!.Id,
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

    [HttpGet("self/company-subscriptions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FollowedDetailedCompanyWithStatsDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCompanySubscriptionsOfSelfStudent(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string orderByExpression = "Name",
        [FromQuery] string searchTerm = "")
    {
        var result = await Mediator.Send(new GetFollowedDetailedCompanyWithStatsSubscriptionsOfStudentForFollowerStudentWithPaginationWithSearchWithFilterWithSortQuery
        {
            FollowerStudentId = AccountInfo!.Id,

            StudentOwnerId = AccountInfo!.Id,

            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm,

            IsCompanyMustBeVerified = true,
            CompanyMustHaveActivationStatus = ActivationStatus.Active,

            StatsFilter = new StatsFilter
            {
                IsJobOfferMustBeActive = true,
                IsSubscriberMustBeVerified = true,
                SubscriberMustHaveActivationStatus = ActivationStatus.Active
            },

            OrderByExpression = orderByExpression,
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    #region Selft

    [HttpPut("self/detail")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateSelfStudentDetail(UpdateOwnStudentDetailView view)
    {
        await Mediator.Send(new UpdateStudentDetailCommand
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

    [HttpPost("self/photo")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid?))]
    public async Task<IActionResult> UpdateSelfStudentPhoto(IFormFile? file)
    {
        return Ok(await Mediator.Send(new UpdateStudentPhotoCommand { StudentId = AccountInfo!.Id, Photo = file }));
    }

    [HttpDelete("self")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteSelfStudent()
    {
        await Mediator.Send(new DeleteStudentCommand(AccountInfo!.Id));

        return NoContent();
    }

    [HttpPost("{studentId}/subscribe")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SubscribeToStudent(Guid studentId)
    {
        await Mediator.Send(new VerifiedActiveStudentOwnerSubscribeToVerifiedActiveStudentTargetCommand
        {
            StudentOwnerId = AccountInfo!.Id,
            StudentTargetId = studentId
        });

        return NoContent();
    }

    [HttpDelete("{studentId}/subscribe")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UnsubscribeFromStudent(Guid studentId)
    {
        await Mediator.Send(new VerifiedActiveStudentOwnerUnsubscribeFromVerifiedActiveStudentTargetCommand
        {
            StudentOwnerId = AccountInfo!.Id,
            StudentTargetId = studentId
        });

        return NoContent();
    }

    #endregion
}
