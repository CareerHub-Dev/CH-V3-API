using API.Authorize;
using API.DTO.Responses;
using Application.Common.DTO.Companies;
using Application.Common.DTO.JobOffers;
using Application.Common.DTO.Students;
using Application.Common.Enums;
using Application.Companies.Commands.DeleteCompany;
using Application.Companies.Commands.InviteCompany;
using Application.Companies.Commands.UpdateCompanyBanner;
using Application.Companies.Commands.UpdateCompanyDetail;
using Application.Companies.Commands.UpdateCompanyLinks;
using Application.Companies.Commands.UpdateCompanyLogo;
using Application.Companies.Queries.GetAmountJobOffersOfCompany;
using Application.Companies.Queries.GetAmountStudentSubscribersOfCompany;
using Application.Companies.Queries.GetBriefCompaniesWithStatsWithPaginig;
using Application.Companies.Queries.GetCompany;
using Application.Emails.Commands.SendInviteCompany;
using Application.JobOffers.Queries.GetDetiledJobOffersWithStatsOfCompanyWithPaging;
using Application.Students.Queries.GetStudentSubscribersOfCompanyWithPaging;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Areas.Admin;

[Authorize(Role.Admin, Role.SuperAdmin)]
[Route("api/Admin/[controller]")]
public class CompaniesController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BriefCompanyWithStatsDTO>))]
    public async Task<IActionResult> GetCompanies(
        [FromQuery] bool? verified,
        [FromQuery] string? order,
        [FromQuery] string? search,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Sender.Send(new GetBriefCompaniesWithStatsWithPagingQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = search ?? string.Empty,
            IsCompanyMustBeVerified = verified,
            OrderByExpression = order ?? "Name"
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("{companyId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CompanyDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCompany(Guid companyId)
    {
        return Ok(await Sender.Send(new GetCompanyQuery
        {
            CompanyId = companyId
        }));
    }

    [HttpGet("{companyId}/amount-subscribers")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountStudentSubscribersOfCompany(Guid companyId)
    {
        return Ok(await Sender.Send(new GetAmountStudentSubscribersOfCompanyQuery
        {
            CompanyId = companyId
        }));
    }

    [HttpGet("{companyId}/amount-jobOffers")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountJobOffersOfCompany(Guid companyId)
    {
        return Ok(await Sender.Send(new GetAmountJobOffersOfCompanyQuery
        {
            CompanyId = companyId
        }));
    }

    [HttpPost("invite")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    public async Task<IActionResult> InviteCompany(InviteCompanyCommand command)
    {
        var result = await Sender.Send(command);

        return Ok(result);
    }

    [HttpPost("send-invite-email")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SendInviteCompanyEmail(SendInviteCompanyEmailCommand command)
    {
        await Sender.Send(command);

        return NoContent();
    }

    [HttpDelete("{companyId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCompany(Guid companyId)
    {
        await Sender.Send(new DeleteCompanyCommand(companyId));

        return NoContent();
    }

    [HttpPut("{companyId}/detail")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateCompanyDetail(Guid companyId, UpdateCompanyDetailCommand command)
    {
        if (companyId != command.CompanyId)
        {
            return BadRequest();
        }

        await Sender.Send(command);

        return NoContent();
    }

    [HttpPut("{companyId}/Links")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateCompanyLinks(Guid companyId, UpdateCompanyLinksCommand command)
    {
        if (companyId != command.CompanyId)
        {
            return BadRequest();
        }

        await Sender.Send(command);

        return NoContent();
    }

    [HttpPost("{companyId}/logo")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImageResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCompanyLogo(Guid companyId, IFormFile? file)
    {
        var result = await Sender.Send(new UpdateCompanyLogoCommand
        {
            CompanyId = companyId,
            Logo = file
        });

        return Ok(new ImageResponse { Route = result });
    }

    [HttpPost("{companyId}/banner")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImageResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCompanyBanner(Guid companyId, IFormFile? file)
    {
        var result = await Sender.Send(new UpdateCompanyBannerCommand
        {
            CompanyId = companyId,
            Banner = file
        });

        return Ok(new ImageResponse { Route = result });
    }

    [HttpGet("{companyId}/JobOffers")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<DetiledJobOfferWithStatsDTO>))]
    public async Task<IActionResult> GetJobOffersOfCompany(
        Guid companyId,
        [FromQuery] bool? active,
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
        var result = await Sender.Send(new GetDetiledJobOffersWithStatsOfCompanyWithPagingQuery
        {
            CompanyId = companyId,

            PageNumber = pageNumber,
            PageSize = pageSize,

            SearchTerm = search ?? string.Empty,

            IsJobOfferMustBeActive = active,
            MustHaveWorkFormat = workFormat,
            MustHaveJobType = jobType,
            MustHaveExperienceLevel = experienceLevel,
            MustHaveJobPositionId = jobPositionId,
            MustHaveTagIds = tagIds,

            OrderByExpression = order ?? "StartDate",
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("{companyId}/student-subscribers")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<StudentDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStudentSubscribersOfCompany(
        Guid companyId,
        [FromQuery] bool? verified,
        [FromQuery] List<Guid>? studentGroupIds,
        [FromQuery] string? order,
        [FromQuery] string? search,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Sender.Send(new GetStudentSubscribersOfCompanyWithPagingQuery
        {
            CompanyId = companyId,

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
}
