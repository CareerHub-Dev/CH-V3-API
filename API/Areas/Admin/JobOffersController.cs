using API.Authorize;
using API.DTO.Responses;
using Application.Common.DTO.CVs;
using Application.Common.DTO.JobOffers;
using Application.Common.DTO.Students;
using Application.Common.Enums;
using Application.CVs.Queries.GetBriefCVsOfStudentWithPaging;
using Application.CVs.Queries.GetBriefCVWithStatussOfJobOfferWithPaging;
using Application.JobOffers.Commands.CreateJobOffer;
using Application.JobOffers.Commands.DeleteJobOffer;
using Application.JobOffers.Commands.UpdateJobOfferDetail;
using Application.JobOffers.Commands.UpdateJobOfferEndDate;
using Application.JobOffers.Commands.UpdateJobOfferImage;
using Application.JobOffers.Queries.GetAmountAppliedCVsOfJobOffer;
using Application.JobOffers.Queries.GetAmountStudentSubscribersOfJobOffer;
using Application.JobOffers.Queries.GetDetiledJobOffersWithStatsWithPaging;
using Application.JobOffers.Queries.GetFollowedDetiledJobOffersWithStatsForFollowerStudentWithPaging;
using Application.JobOffers.Queries.GetJobOffer;
using Application.JobOffers.Queries.Models;
using Application.Students.Queries.GetStudentSubscribersOfCompanyWithPaging;
using Application.Students.Queries.GetStudentSubscribersOfJobOfferWithPaging;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Areas.Admin;

[Authorize(Role.Admin, Role.SuperAdmin)]
[Route("api/Admin/[controller]")]
public class JobOffersController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<DetiledJobOfferWithStatsDTO>))]
    public async Task<IActionResult> GetJobOffers(
        [FromQuery] string? order,
        [FromQuery] string? search,
        [FromQuery] JobType? jobType,
        [FromQuery] WorkFormat? workFormat,
        [FromQuery] ExperienceLevel? experienceLevel,
        [FromQuery] Guid? jobPositionId,
        [FromQuery] List<Guid>? tagIds,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Sender.Send(new GetDetiledJobOffersWithStatsWithPagingQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,

            SearchTerm = search ?? string.Empty,

            IsJobOfferMustBeActive = true,
            MustHaveWorkFormat = workFormat,
            MustHaveJobType = jobType,
            MustHaveExperienceLevel = experienceLevel,
            MustHaveJobPositionId = jobPositionId,
            MustHaveTagIds = tagIds,
            IsCompanyOfJobOfferMustBeVerified = true,

            OrderByExpression = order ?? "StartDate",
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("{jobOfferId}/amount-student-subscribers")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountStudentSubscribersOfJobOffer(Guid jobOfferId)
    {
        return Ok(await Sender.Send(new GetAmountStudentSubscribersOfJobOfferQuery
        {
            JobOfferId = jobOfferId
        }));
    }

    [HttpGet("{jobOfferId}/amount-applied-cvs")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountAppliedCVsOfJobOffer(Guid jobOfferId)
    {
        return Ok(await Sender.Send(new GetAmountAppliedCVsOfJobOfferQuery
        {
            JobOfferId = jobOfferId
        }));
    }

    [HttpGet("{jobOfferId}/student-subscribers")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<StudentDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStudentSubscribersOfJobOffers(
        Guid jobOfferId,
        [FromQuery] bool? verified,
        [FromQuery] List<Guid>? studentGroupIds,
        [FromQuery] string? order,
        [FromQuery] string? search,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Sender.Send(new GetStudentSubscribersOfJobOfferWithPagingQuery
        {
            JobOfferId = jobOfferId,

            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = search ?? string.Empty,

            IsStudentSubscriberMustBeVerified = verified,
            StudentGroupIds = studentGroupIds,

            OrderByExpression = order ?? "LastName",
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("{jobOfferId}/CVs")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BriefCVWithStatusDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCVsOfStudent(
        Guid jobOfferId,
        [FromQuery] string? order,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Sender.Send(new GetBriefCVWithStatussOfJobOfferWithPagingQuery
        {
            JobOfferId = jobOfferId,

            PageNumber = pageNumber,
            PageSize = pageSize,

            OrderByExpression = order ?? "Title"
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("{jobOfferId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JobOfferDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetJobOffer(Guid jobOfferId)
    {
        return Ok(await Sender.Send(new GetJobOfferQuery
        {
            JobOfferId = jobOfferId
        }));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateJobOffer([FromForm] CreateJobOfferCommand command)
    {
        var result = await Sender.Send(command);

        return Ok(result);
    }

    [HttpDelete("{jobOfferId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteJobOffer(Guid jobOfferId)
    {
        await Sender.Send(new DeleteJobOfferCommand(jobOfferId));

        return NoContent();
    }

    [HttpPut("{jobOfferId}/detail")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateJobOfferDetail(Guid jobOfferId, UpdateJobOfferDetailCommand command)
    {
        if (jobOfferId != command.JobOfferId)
        {
            return BadRequest();
        }

        await Sender.Send(command);

        return NoContent();
    }

    [HttpPut("{jobOfferId}/endDate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateJobOfferEndDate(Guid jobOfferId, UpdateJobOfferEndDateCommand command)
    {
        if (jobOfferId != command.JobOfferId)
        {
            return BadRequest();
        }

        await Sender.Send(command);

        return NoContent();
    }

    [HttpPost("{jobOfferId}/image")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImageResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateJobOfferImage(Guid jobOfferId, IFormFile? file)
    {
        var result = await Sender.Send(new UpdateJobOfferImageCommand
        {
            JobofferId = jobOfferId,
            Image = file
        });

        return Ok(new ImageResponse { 
            Route = result 
        });
    }
}
