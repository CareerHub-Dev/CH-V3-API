using Application.JobOffers.Commands.CreateJobOffer;
using Application.JobOffers.Commands.DeleteJobOfferOfCompany;
using Application.JobOffers.Commands.UpdateJobOfferDetailOfCompany;
using Application.JobOffers.Commands.UpdateJobOfferImageOfCompany;
using Microsoft.AspNetCore.Mvc;
using API.Authorize;
using API.DTO.Requests.JobOffers;
using Application.JobOffers.Queries.GetAmount;
using Domain.Enums;
using Application.Common.DTO.JobOffers;
using Application.JobOffers.Queries.GetJobOffer;
using Application.JobOffers.Queries.GetJobOffers;
using Newtonsoft.Json;

using JobOfferStatsFilter = Application.JobOffers.Queries.Models.StatsFilter;

namespace API.Areas.Company;

[Authorize("Company")]
[Route("api/Company/self/[controller]")]
public class JobOffersController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<DetiledJobOfferWithStatsDTO>))]
    public async Task<IActionResult> GetJobOffersOfSelfCompany(
        [FromQuery] string? orderByExpression,
        [FromQuery] string? searchTerm,
        [FromQuery] bool? isJobOfferMustBeActive,
        [FromQuery] JobType? mustHaveJobType,
        [FromQuery] WorkFormat? mustHaveWorkFormat,
        [FromQuery] ExperienceLevel? mustHaveExperienceLevel,
        [FromQuery] Guid? mustHavejobPositionId,
        [FromQuery] List<Guid>? mustHaveTagIds,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Mediator.Send(new GetDetiledJobOffersWithStatsOfCompanyWithPaginationWithSearchWithFilterWithSortQuery
        {
            CompanyId = AccountInfo!.Id,

            PageNumber = pageNumber,
            PageSize = pageSize,

            SearchTerm = searchTerm ?? string.Empty,

            IsJobOfferMustBeActive = isJobOfferMustBeActive,
            MustHaveWorkFormat = mustHaveWorkFormat,
            MustHaveJobType = mustHaveJobType,
            MustHaveExperienceLevel = mustHaveExperienceLevel,
            MustHaveJobPositionId = mustHavejobPositionId,
            MustHaveTagIds = mustHaveTagIds,

            StatsFilter = new JobOfferStatsFilter
            {
                IsStudentOfAppliedCVMustBeVerified = true,
                StudentOfCVMustHaveActivationStatus = ActivationStatus.Active,

                IsSubscriberMustBeVerified = true,
                SubscriberMustHaveActivationStatus = ActivationStatus.Active
            },

            OrderByExpression = orderByExpression ?? "StartDate",
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("{jobOfferId}/amount-student-subscribers")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountStudentSubscribersOfSelfJobOffer(Guid jobOfferId)
    {
        return Ok(await Mediator.Send(new GetAmountStudentSubscribersOfCompanyJobOfferWithFilterQuery
        {
            JobOfferId = jobOfferId,
            CompanyId = AccountInfo!.Id,

            IsSubscriberMustBeVerified = true,
            SubscriberMustHaveActivationStatus = ActivationStatus.Active
        }));
    }

    [HttpGet("{jobOfferId}/amount-applied-cvs")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountAppliedCVsOfSelfJobOffer(Guid jobOfferId)
    {
        return Ok(await Mediator.Send(new GetAmountAppliedCVsOfCompanyJobOfferWithFilterQuery
        {
            JobOfferId = jobOfferId,
            CompanyId = AccountInfo!.Id,

            IsStudentOfAppliedCVMustBeVerified = true,
            StudentOfCVMustHaveActivationStatus = ActivationStatus.Active
        }));
    }

    [HttpGet("{jobOfferId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JobOfferDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetJobOffer(Guid jobOfferId)
    {
        return Ok(await Mediator.Send(new GetJobOfferOfCompanyWithFilterQuery
        {
            JobOfferId = jobOfferId,
            CompanyId = AccountInfo!.Id
        }));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateJobOfferForSelfCompany(CreateJobOfferRequest view)
    {
        var result = await Mediator.Send(new CreateJobOfferCommand
        {
            Title = view.Title,
            Overview = view.Overview,
            Requirements = view.Requirements,
            Responsibilities = view.Responsibilities,
            Preferences = view.Preferences,
            Image = view.Image,
            JobType = view.JobType,
            WorkFormat = view.WorkFormat,
            ExperienceLevel = view.ExperienceLevel,
            StartDate = view.StartDate,
            EndDate = view.EndDate,
            JobPositionId = view.JobPositionId,
            TagIds = view.TagIds,

            CompanyId = AccountInfo!.Id
        });

        return Ok(result);
    }

    [HttpDelete("{jobOfferId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteJobOfferOfSelfCompany(Guid jobOfferId)
    {
        await Mediator.Send(new DeleteJobOfferOfCompanyCommand(jobOfferId, AccountInfo!.Id));

        return NoContent();
    }

    [HttpPut("{jobOfferId}/detail")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateJobOfferDetailOfSelfCompany(Guid jobOfferId, UpdateJobOfferDetailRequest view)
    {
        if (jobOfferId != view.JobOfferId)
        {
            return BadRequest();
        }

        await Mediator.Send(new UpdateJobOfferDetailOfCompanyCommand
        {
            JobOfferId = view.JobOfferId,
            Title = view.Title,
            Overview = view.Overview,
            Requirements = view.Requirements,
            Responsibilities = view.Responsibilities,
            Preferences = view.Preferences,
            JobType = view.JobType,
            WorkFormat = view.WorkFormat,
            ExperienceLevel = view.ExperienceLevel,
            StartDate = view.StartDate,
            EndDate = view.EndDate,
            JobPositionId = view.JobPositionId,
            TagIds = view.TagIds,

            CompanyId = AccountInfo!.Id
        });

        return NoContent();
    }

    [HttpPost("{jobOfferId}/image")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid?))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateJobOfferImageOfSelfCompany(Guid jobOfferId, IFormFile? file)
    {
        var result = await Mediator.Send(new UpdateJobOfferImageOfCompanyCommand 
        { 
            CompanyId = AccountInfo!.Id, 

            JobofferId = jobOfferId,
            Image = file 
        });

        return Ok(result);
    }
}
