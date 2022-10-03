using Application.Common.DTO.Companies;
using Application.Common.DTO.Experiences;
using Application.Common.DTO.Students;
using Application.Companies.Queries.GetCompanySubscriptionsOfStudent;
using Application.Companies.Queries.Models;
using Application.Experiences.Queries.GetExperiences;
using Application.Students.Commands.DeleteStudent;
using Application.Students.Commands.UpdateStudentDetail;
using Application.Students.Commands.UpdateStudentPhoto;
using Application.Students.Commands.VerifiedActiveStudentOwnerSubscribeToVerifiedActiveStudentTarget;
using Application.Students.Commands.VerifiedActiveStudentOwnerUnsubscribeFromVerifiedActiveStudentTarget;
using Application.Students.Queries;
using Application.Students.Queries.GetAmount;
using Application.Students.Queries.GetStudent;
using Application.Students.Queries.GetStudents;
using Application.Students.Queries.GetStudentSubscriptionsOfStudent;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebUI.Authorize;
using WebUI.ViewModels.Students;

namespace WebUI.Areas.Student;

[Authorize("Student")]
[Route("api/Student/[controller]")]
public class StudentsController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FollowedDetailedStudentDTO>))]
    public async Task<IActionResult> GetStudents(
        [FromQuery] ActivationStatus? studentMustHaveActivationStatus,
        [FromQuery] List<Guid>? studentGroupIds,
        [FromQuery] string? orderByExpression,
        [FromQuery] string? searchTerm,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Mediator.Send(new GetFollowedDetailedStudentsForFollowerStudentWithPaginationWithSearchWithFilterWithSortQuery
        {
            FollowerStudentId = AccountInfo!.Id,

            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm ?? string.Empty,

            IsStudentMustBeVerified = true,
            WithoutStudentId = AccountInfo!.Id,
            StudentGroupIds = studentGroupIds,
            StudentMustHaveActivationStatus = studentMustHaveActivationStatus,

            OrderByExpression = orderByExpression ?? "LastName",
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("{studentId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DetailedStudentDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStudent(Guid studentId)
    {
        return Ok(await Mediator.Send(new GetDetailedStudentWithFilterQuery
        {
            StudentId = studentId,
            IsStudentMustBeVerified = true,
            StudentMustHaveActivationStatus = ActivationStatus.Active
        }));
    }

    [HttpGet("{studentId}/amount-company-subscriptions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DetailedStudentDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountCompanySubscriptionsOfStudent(Guid studentId)
    {
        return Ok(await Mediator.Send(new GetAmountCompanySubscriptionsOfStudentWithFilterQuery
        {
            StudentId = studentId,
            IsStudentMustBeVerified = true,
            StudentMustHaveActivationStatus = ActivationStatus.Active,

            IsCompanyMustBeVerified = true,
            CompanyMustHaveActivationStatus = ActivationStatus.Active
        }));
    }

    [HttpGet("{studentId}/amount-jobOffer-subscriptions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DetailedStudentDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountJobOfferSubscriptionsOfStudent(Guid studentId)
    {
        return Ok(await Mediator.Send(new GetAmountJobOfferSubscriptionsOfStudentWithFilterQuery
        {
            StudentId = studentId,
            IsStudentMustBeVerified = true,
            StudentMustHaveActivationStatus = ActivationStatus.Active,

            IsJobOfferMustBeActive = true,
            IsCompanyOfJobOfferMustBeVerified = true,
            CompanyOfJobOfferMustHaveActivationStatus = ActivationStatus.Active
        }));
    }

    [HttpGet("{studentId}/amount-student-subscriptions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DetailedStudentDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountStudentSubscriptionsOfStudent(Guid studentId)
    {
        return Ok(await Mediator.Send(new GetAmountStudentSubscriptionsOfStudentWithFilterQuery
        {
            StudentId = studentId,
            IsStudentMustBeVerified = true,
            StudentMustHaveActivationStatus = ActivationStatus.Active,

            IsStudentTargetOfSubscriptionMustBeVerified = true,
            StudentTargetOfSubscriptionMustHaveActivationStatus = ActivationStatus.Active
        }));
    }

    [HttpGet("{studentId}/student-subscriptions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FollowedDetailedStudentDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStudentSubscriptionsOfStudent(
        Guid studentId,
        [FromQuery] List<Guid>? studentGroupIds,
        [FromQuery] string? orderByExpression,
        [FromQuery] string? searchTerm,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Mediator.Send(new GetFollowedDetailedStudentSubsciptionsOfStudentOwnerForFollowerStudentWithPaginationWithSearchWithFilterWithSortQuery
        {
            FollowerStudentId = AccountInfo!.Id,

            StudentOwnerId = studentId,
            IsStudentOwnerMustBeVerified = true,
            StudentOwnerMustHaveActivationStatus = ActivationStatus.Active,

            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm ?? string.Empty,

            IsStudentMustBeVerified = true,
            WithoutStudentId = AccountInfo!.Id,
            StudentGroupIds = studentGroupIds,
            StudentMustHaveActivationStatus = ActivationStatus.Active,

            OrderByExpression = orderByExpression ?? "LastName"
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("{studentId}/company-subscriptions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FollowedDetailedCompanyWithStatsDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCompanySubscriptionsOfStudent(
        Guid studentId,
        [FromQuery] string? orderByExpression,
        [FromQuery] string? searchTerm,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
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
            SearchTerm = searchTerm ?? string.Empty,

            IsCompanyMustBeVerified = true,
            CompanyMustHaveActivationStatus = ActivationStatus.Active,

            StatsFilter = new StatsFilter
            {
                IsJobOfferMustBeActive = true,

                IsSubscriberMustBeVerified = true,
                SubscriberMustHaveActivationStatus = ActivationStatus.Active
            },

            OrderByExpression = orderByExpression ?? "Name",
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
        var result = await Mediator.Send(new GetExperiencesOfStudentWithPaginationWithFilterWithSortQuery
        {
            StudentId = studentId,
            IsStudentMustBeVerified = true,
            PageNumber = pageNumber,
            PageSize = pageSize,
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    #region Selft

    [HttpGet("self")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DetailedStudentDTO))]
    public async Task<DetailedStudentDTO> GetSelfStudent()
    {
        return await Mediator.Send(new GetDetailedStudentWithFilterQuery
        {
            StudentId = AccountInfo!.Id
        });
    }

    [HttpPut("self/detail")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

    [HttpGet("{studentId}/subscribe")]
    public async Task<bool> IsStudentOwnerSubscribedToStudentTarget(Guid studentId)
    {
        var result = await Mediator.Send(new IsVerifiedActiveStudentOwnerSubscribedToVerifiedActiveStudentTargetQuery
        {
            StudentOwnerId = AccountInfo!.Id,
            StudentTargetId = studentId,
        });

        return result;
    }

    [HttpGet("self/student-subscriptions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FollowedDetailedStudentDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStudentSubscriptionsOfSelfStudent(
        [FromQuery] List<Guid>? studentGroupIds,
        [FromQuery] string? orderByExpression,
        [FromQuery] string? searchTerm,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Mediator.Send(new GetFollowedDetailedStudentSubsciptionsOfStudentOwnerForFollowerStudentWithPaginationWithSearchWithFilterWithSortQuery
        {
            FollowerStudentId = AccountInfo!.Id,

            StudentOwnerId = AccountInfo!.Id,

            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm ?? string.Empty,

            IsStudentMustBeVerified = true,
            StudentGroupIds = studentGroupIds,
            StudentMustHaveActivationStatus = ActivationStatus.Active,
            WithoutStudentId = AccountInfo!.Id,

            OrderByExpression = orderByExpression ?? "LastName",
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("self/company-subscriptions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FollowedDetailedCompanyWithStatsDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCompanySubscriptionsOfSelfStudent(
        [FromQuery] string? orderByExpression,
        [FromQuery] string? searchTerm,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Mediator.Send(new GetFollowedDetailedCompanyWithStatsSubscriptionsOfStudentForFollowerStudentWithPaginationWithSearchWithFilterWithSortQuery
        {
            FollowerStudentId = AccountInfo!.Id,

            StudentOwnerId = AccountInfo!.Id,

            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm ?? string.Empty,

            IsCompanyMustBeVerified = true,
            CompanyMustHaveActivationStatus = ActivationStatus.Active,

            StatsFilter = new StatsFilter
            {
                IsJobOfferMustBeActive = true,
                IsSubscriberMustBeVerified = true,
                SubscriberMustHaveActivationStatus = ActivationStatus.Active
            },

            OrderByExpression = orderByExpression ?? "Name",
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("self/amount-company-subscriptions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountCompanySubscriptionsOfSelfStudent()
    {
        return Ok(await Mediator.Send(new GetAmountCompanySubscriptionsOfStudentWithFilterQuery
        {
            StudentId = AccountInfo!.Id,

            IsCompanyMustBeVerified = true,
            CompanyMustHaveActivationStatus = ActivationStatus.Active
        }));
    }

    [HttpGet("self/amount-jobOffer-subscriptions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountJobOfferSubscriptionsOfSelfStudent()
    {
        return Ok(await Mediator.Send(new GetAmountJobOfferSubscriptionsOfStudentWithFilterQuery
        {
            StudentId = AccountInfo!.Id,

            IsJobOfferMustBeActive = true,
            IsCompanyOfJobOfferMustBeVerified = true,
            CompanyOfJobOfferMustHaveActivationStatus = ActivationStatus.Active
        }));
    }

    [HttpGet("self/amount-student-subscriptions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountStudentSubscriptionsOfSelfStudent()
    {
        return Ok(await Mediator.Send(new GetAmountStudentSubscriptionsOfStudentWithFilterQuery
        {
            StudentId = AccountInfo!.Id,

            IsStudentTargetOfSubscriptionMustBeVerified = true,
            StudentTargetOfSubscriptionMustHaveActivationStatus = ActivationStatus.Active
        }));
    }

    [HttpGet("self/amount-cvs")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountCVsOfSelfStudent()
    {
        return Ok(await Mediator.Send(new GetAmountCVsOfStudentWithFilterQuery
        {
            StudentId = AccountInfo!.Id,
        }));
    }

    #endregion
}
