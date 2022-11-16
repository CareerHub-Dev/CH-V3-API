using API.Authorize;
using API.DTO.Requests.Students;
using Application.Common.DTO.Companies;
using Application.Common.DTO.Experiences;
using Application.Common.DTO.JobOffers;
using Application.Common.DTO.Students;
using Application.Common.Enums;
using Application.Companies.Queries.GetFollowedShortCompanySubscriptionsWithStatsOfStudentForFollowerStudentWithPaging;
using Application.Experiences.Queries.GetExperiencesOfStudentWithPaging;
using Application.JobOffers.Queries.GetJobOfferSubscriptionsOfStudent;
using Application.Students.Commands.ReturnStudentToLog;
using Application.Students.Commands.UpdateStudentDetail;
using Application.Students.Commands.UpdateStudentPhoto;
using Application.Students.Commands.VerifiedStudentOwnerSubscribeToVerifiedStudentTarget;
using Application.Students.Commands.VerifiedStudentOwnerUnsubscribeFromVerifiedStudentTarget;
using Application.Students.Queries.GetAmountCompanySubscriptionsOfStudent;
using Application.Students.Queries.GetAmountCVsOfStudent;
using Application.Students.Queries.GetAmountJobOfferSubscriptionsOfStudent;
using Application.Students.Queries.GetAmountStudentSubscribersOfStudent;
using Application.Students.Queries.GetAmountStudentSubscriptionsOfStudent;
using Application.Students.Queries.GetDetailedStudent;
using Application.Students.Queries.GetFollowedShortStudentsForFollowerStudentWithPaging;
using Application.Students.Queries.GetFollowedShortStudentSubscribersOfStudentForFollowerStudentWithPaging;
using Application.Students.Queries.GetFollowedShortStudentSubscriptionsOfStudentForFollowerStudentWithPaging;
using Application.Students.Queries.IsVerifiedStudentOwnerSubscribedToVerifiedStudentTarget;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using CompanyStatsFilter = Application.Companies.Queries.Models.StatsFilter;
using JobOfferStatsFilter = Application.JobOffers.Queries.Models.StatsFilter;

namespace API.Areas.Student;

[Authorize(Role.Student)]
[Route("api/Student/[controller]")]
public class StudentsController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FollowedShortStudentDTO>))]
    public async Task<IActionResult> GetStudents(
        [FromQuery] List<Guid>? studentGroupIds,
        [FromQuery] string? orderByExpression,
        [FromQuery] string? searchTerm,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Sender.Send(new GetFollowedShortStudentsForFollowerStudentWithPagingQuery
        {
            FollowerStudentId = AccountInfo!.Id,

            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm ?? string.Empty,

            IsStudentMustBeVerified = true,
            WithoutStudentId = AccountInfo!.Id,
            StudentGroupIds = studentGroupIds,

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
        return Ok(await Sender.Send(new GetDetailedStudentQuery
        {
            StudentId = studentId,
            IsStudentMustBeVerified = true,
        }));
    }

    [HttpGet("{studentId}/amount-company-subscriptions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountCompanySubscriptionsOfStudent(Guid studentId)
    {
        return Ok(await Sender.Send(new GetAmountCompanySubscriptionsOfStudentQuery
        {
            StudentId = studentId,
            IsStudentMustBeVerified = true,

            IsCompanyMustBeVerified = true,
        }));
    }

    [HttpGet("{studentId}/amount-jobOffer-subscriptions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountJobOfferSubscriptionsOfStudent(Guid studentId)
    {
        return Ok(await Sender.Send(new GetAmountJobOfferSubscriptionsOfStudentQuery
        {
            StudentId = studentId,
            IsStudentMustBeVerified = true,

            IsJobOfferMustBeActive = true,
            IsCompanyOfJobOfferMustBeVerified = true,
        }));
    }

    [HttpGet("{studentId}/amount-student-subscriptions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountStudentSubscriptionsOfStudent(Guid studentId)
    {
        return Ok(await Sender.Send(new GetAmountStudentSubscriptionsOfStudentQuery
        {
            StudentId = studentId,
            IsStudentMustBeVerified = true,

            IsStudentTargetOfSubscriptionMustBeVerified = true,
        }));
    }

    [HttpGet("{studentId}/amount-student-subscribers")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountStudentSubscribersOfStudent(Guid studentId)
    {
        return Ok(await Sender.Send(new GetAmountStudentSubscribersOfStudentQuery
        {
            StudentId = studentId,
            IsStudentMustBeVerified = true,

            IsStudentOwnerOfSubscriptionMustBeVerified = true,
        }));
    }

    [HttpGet("{studentId}/subscribe")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<bool> IsStudentOwnerSubscribedToStudentTarget(Guid studentId)
    {
        var result = await Sender.Send(new IsVerifiedStudentOwnerSubscribedToVerifiedStudentTargetQuery
        {
            StudentOwnerId = AccountInfo!.Id,
            StudentTargetId = studentId,
        });

        return result;
    }

    [HttpPost("{studentId}/subscribe")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SubscribeToStudent(Guid studentId)
    {
        await Sender.Send(new VerifiedStudentOwnerSubscribeToVerifiedStudentTargetCommand
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
        await Sender.Send(new VerifiedStudentOwnerUnsubscribeFromVerifiedStudentTargetCommand
        {
            StudentOwnerId = AccountInfo!.Id,
            StudentTargetId = studentId
        });

        return NoContent();
    }

    [HttpGet("{studentId}/student-subscriptions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FollowedShortStudentDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStudentSubscriptionsOfStudent(
        Guid studentId,
        [FromQuery] List<Guid>? studentGroupIds,
        [FromQuery] string? orderByExpression,
        [FromQuery] string? searchTerm,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Sender.Send(new GetFollowedShortStudentSubscriptionsOfStudentForFollowerStudentWithPagingQuery
        {
            FollowerStudentId = AccountInfo!.Id,

            StudentId = studentId,
            IsStudentMustBeVerified = true,

            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm ?? string.Empty,

            IsStudentSubscriptionMustBeVerified = true,
            WithoutStudentSubscriptionId = AccountInfo!.Id,
            StudentGroupIds = studentGroupIds,

            OrderByExpression = orderByExpression ?? "LastName"
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("{studentId}/student-subscribers")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FollowedShortStudentDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStudentSubscribersOfStudent(
        Guid studentId,
        [FromQuery] List<Guid>? studentGroupIds,
        [FromQuery] string? orderByExpression,
        [FromQuery] string? searchTerm,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Sender.Send(new GetFollowedShortStudentSubscribersOfStudentForFollowerStudentWithPagingQuery
        {
            FollowerStudentId = AccountInfo!.Id,

            StudentId = studentId,
            IsStudentMustBeVerified = true,

            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm ?? string.Empty,

            IsStudentSubscriberMustBeVerified = true,
            WithoutStudentSubscriberId = AccountInfo!.Id,
            StudentGroupIds = studentGroupIds,

            OrderByExpression = orderByExpression ?? "LastName"
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("{studentId}/company-subscriptions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FollowedShortCompanyWithStatsDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCompanySubscriptionsOfStudent(
        Guid studentId,
        [FromQuery] string? orderByExpression,
        [FromQuery] string? searchTerm,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Sender.Send(new GetFollowedShortCompanySubscriptionsWithStatsOfStudentForFollowerStudentWithPagingQuery
        {
            FollowerStudentId = AccountInfo!.Id,
            IsFollowerStudentMustBeVerified = true,

            StudentId = studentId,
            IsStudentMustBeVerified = true,

            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm ?? string.Empty,

            IsCompanyMustBeVerified = true,

            StatsFilter = new CompanyStatsFilter
            {
                IsJobOfferMustBeActive = true,

                IsSubscriberMustBeVerified = true,
            },

            OrderByExpression = orderByExpression ?? "Name",
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("{studentId}/jobOffer-subscriptions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FollowedDetiledJobOfferWithStatsDTO>))]
    public async Task<IActionResult> GetJobOfferSubscriptionsOfStudent(
        Guid studentId,
        [FromQuery] string? orderByExpression,
        [FromQuery] string? searchTerm,
        [FromQuery] JobType? mustHaveJobType,
        [FromQuery] WorkFormat? mustHaveWorkFormat,
        [FromQuery] ExperienceLevel? mustHaveExperienceLevel,
        [FromQuery] Guid? mustHavejobPositionId,
        [FromQuery] List<Guid>? mustHaveTagIds,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Sender.Send(new GetFollowedDetiledJobOfferSubscriptionsWithStatsOfStudentOwnerForFollowerStudentWithPaginationWithSearchWithFilterWithSortQuery
        {
            FollowerStudentId = AccountInfo!.Id,

            StudentOwnerId = studentId,
            IsStudentOwnerMustBeVerified = true,

            PageNumber = pageNumber,
            PageSize = pageSize,

            SearchTerm = searchTerm ?? string.Empty,

            IsJobOfferMustBeActive = true,
            MustHaveWorkFormat = mustHaveWorkFormat,
            MustHaveJobType = mustHaveJobType,
            MustHaveExperienceLevel = mustHaveExperienceLevel,
            MustHaveJobPositionId = mustHavejobPositionId,
            MustHaveTagIds = mustHaveTagIds,
            IsCompanyOfJobOfferMustBeVerified = true,

            StatsFilter = new JobOfferStatsFilter
            {
                IsStudentOfAppliedCVMustBeVerified = true,

                IsSubscriberMustBeVerified = true,
            },

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
            IsStudentMustBeVerified = true,

            PageNumber = pageNumber,
            PageSize = pageSize,

            OrderByExpression = orderByExpression ?? "Title"
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    #region Self

    [HttpGet("self")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DetailedStudentDTO))]
    public async Task<DetailedStudentDTO> GetSelfStudent()
    {
        return await Sender.Send(new GetDetailedStudentQuery
        {
            StudentId = AccountInfo!.Id
        });
    }

    [HttpPut("self/detail")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateSelfStudentDetail(UpdateOwnStudentDetailRequest request)
    {
        await Sender.Send(new UpdateStudentDetailCommand
        {
            StudentId = AccountInfo!.Id,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Phone = request.Phone,
            BirthDate = request.BirthDate,
            StudentGroupId = request.StudentGroupId,
        });

        return NoContent();
    }

    [HttpPost("self/photo")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateSelfStudentPhoto(IFormFile? file)
    {
        return Ok(await Sender.Send(new UpdateStudentPhotoCommand { StudentId = AccountInfo!.Id, Photo = file }));
    }

    [HttpDelete("self")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteSelfStudent()
    {
        await Sender.Send(new ReturnStudentToLogCommand(AccountInfo!.Id));

        return NoContent();
    }

    [HttpGet("self/student-subscriptions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FollowedShortStudentDTO>))]
    public async Task<IActionResult> GetStudentSubscriptionsOfSelfStudent(
        [FromQuery] List<Guid>? studentGroupIds,
        [FromQuery] string? orderByExpression,
        [FromQuery] string? searchTerm,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Sender.Send(new GetFollowedShortStudentSubscriptionsOfStudentForFollowerStudentWithPagingQuery
        {
            FollowerStudentId = AccountInfo!.Id,

            StudentId = AccountInfo!.Id,

            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm ?? string.Empty,

            IsStudentSubscriptionMustBeVerified = true,
            StudentGroupIds = studentGroupIds,
            WithoutStudentSubscriptionId = AccountInfo!.Id,

            OrderByExpression = orderByExpression ?? "LastName",
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("self/student-subscribers")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FollowedShortStudentDTO>))]
    public async Task<IActionResult> GetStudentSubscribersOfSelfStudent(
        [FromQuery] List<Guid>? studentGroupIds,
        [FromQuery] string? orderByExpression,
        [FromQuery] string? searchTerm,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Sender.Send(new GetFollowedShortStudentSubscribersOfStudentForFollowerStudentWithPagingQuery
        {
            FollowerStudentId = AccountInfo!.Id,

            StudentId = AccountInfo!.Id,

            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm ?? string.Empty,

            IsStudentSubscriberMustBeVerified = true,
            StudentGroupIds = studentGroupIds,
            WithoutStudentSubscriberId = AccountInfo!.Id,

            OrderByExpression = orderByExpression ?? "LastName",
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("self/company-subscriptions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FollowedShortCompanyWithStatsDTO>))]
    public async Task<IActionResult> GetCompanySubscriptionsOfSelfStudent(
        [FromQuery] string? orderByExpression,
        [FromQuery] string? searchTerm,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Sender.Send(new GetFollowedShortCompanySubscriptionsWithStatsOfStudentForFollowerStudentWithPagingQuery
        {
            FollowerStudentId = AccountInfo!.Id,

            StudentId = AccountInfo!.Id,

            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm ?? string.Empty,

            IsCompanyMustBeVerified = true,

            StatsFilter = new CompanyStatsFilter
            {
                IsJobOfferMustBeActive = true,
                IsSubscriberMustBeVerified = true,
            },

            OrderByExpression = orderByExpression ?? "Name",
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("self/jobOffer-subscriptions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FollowedDetiledJobOfferWithStatsDTO>))]
    public async Task<IActionResult> GetJobOfferSubscriptionsOfSelfStudent(
        [FromQuery] string? orderByExpression,
        [FromQuery] string? searchTerm,
        [FromQuery] JobType? mustHaveJobType,
        [FromQuery] WorkFormat? mustHaveWorkFormat,
        [FromQuery] ExperienceLevel? mustHaveExperienceLevel,
        [FromQuery] Guid? mustHavejobPositionId,
        [FromQuery] List<Guid>? mustHaveTagIds,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Sender.Send(new GetFollowedDetiledJobOfferSubscriptionsWithStatsOfStudentOwnerForFollowerStudentWithPaginationWithSearchWithFilterWithSortQuery
        {
            FollowerStudentId = AccountInfo!.Id,

            StudentOwnerId = AccountInfo.Id,

            PageNumber = pageNumber,
            PageSize = pageSize,

            SearchTerm = searchTerm ?? string.Empty,

            IsJobOfferMustBeActive = true,
            MustHaveWorkFormat = mustHaveWorkFormat,
            MustHaveJobType = mustHaveJobType,
            MustHaveExperienceLevel = mustHaveExperienceLevel,
            MustHaveJobPositionId = mustHavejobPositionId,
            MustHaveTagIds = mustHaveTagIds,

            StatsFilter = new JobOfferStatsFilter
            {
                IsStudentOfAppliedCVMustBeVerified = true,

                IsSubscriberMustBeVerified = true,
            },

            OrderByExpression = orderByExpression ?? "StartDate",
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("self/amount-company-subscriptions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    public async Task<IActionResult> GetAmountCompanySubscriptionsOfSelfStudent()
    {
        return Ok(await Sender.Send(new GetAmountCompanySubscriptionsOfStudentQuery
        {
            StudentId = AccountInfo!.Id,

            IsCompanyMustBeVerified = true,
        }));
    }

    [HttpGet("self/amount-jobOffer-subscriptions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    public async Task<IActionResult> GetAmountJobOfferSubscriptionsOfSelfStudent()
    {
        return Ok(await Sender.Send(new GetAmountJobOfferSubscriptionsOfStudentQuery
        {
            StudentId = AccountInfo!.Id,

            IsJobOfferMustBeActive = true,
            IsCompanyOfJobOfferMustBeVerified = true,
        }));
    }

    [HttpGet("self/amount-student-subscriptions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    public async Task<IActionResult> GetAmountStudentSubscriptionsOfSelfStudent()
    {
        return Ok(await Sender.Send(new GetAmountStudentSubscriptionsOfStudentQuery
        {
            StudentId = AccountInfo!.Id,

            IsStudentTargetOfSubscriptionMustBeVerified = true,
        }));
    }

    [HttpGet("self/amount-student-subscribers")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    public async Task<IActionResult> GetAmountStudentSubscribersOfStudent()
    {
        return Ok(await Sender.Send(new GetAmountStudentSubscribersOfStudentQuery
        {
            StudentId = AccountInfo!.Id,

            IsStudentOwnerOfSubscriptionMustBeVerified = true,
        }));
    }

    [HttpGet("self/amount-cvs")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    public async Task<IActionResult> GetAmountCVsOfSelfStudent()
    {
        return Ok(await Sender.Send(new GetAmountCVsOfStudentQuery
        {
            StudentId = AccountInfo!.Id,
        }));
    }

    #endregion
}
