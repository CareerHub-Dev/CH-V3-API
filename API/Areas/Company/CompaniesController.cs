using API.Authorize;
using API.DTO.Requests.Companies;
using API.DTO.Responses;
using Application.Common.DTO.Companies;
using Application.Common.Enums;
using Application.Companies.Commands.DeleteCompany;
using Application.Companies.Commands.UpdateCompanyBanner;
using Application.Companies.Commands.UpdateCompanyDetail;
using Application.Companies.Commands.UpdateCompanyLinks;
using Application.Companies.Commands.UpdateCompanyLogo;
using Application.Companies.Queries.GetAmountJobOffersOfCompany;
using Application.Companies.Queries.GetAmountStudentSubscribersOfCompany;
using Application.Companies.Queries.GetDetailedCompany;
using Microsoft.AspNetCore.Mvc;

namespace API.Areas.Company;

[Authorize(Role.Company)]
[Route("api/Company/[controller]")]
public class CompaniesController : ApiControllerBase
{
    [HttpGet("self")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DetailedCompanyDTO))]
    public async Task<IActionResult> GetSelfCompany()
    {
        return Ok(await Sender.Send(new GetDetailedCompanyQuery
        {
            CompanyId = AccountInfo!.Id
        }));
    }

    [HttpPut("self/detail")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateSelfCompanyDetailAccount(UpdateOwnCompanyDetailRequest request)
    {
        await Sender.Send(new UpdateCompanyDetailCommand
        {
            CompanyId = AccountInfo!.Id,
            Motto = request.Motto,
            Description = request.Description,
            Name = request.Name,
        });

        return NoContent();
    }

    [HttpPut("self/Links")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateCompanyLinks(UpdateOwnCompanyLinksRequest command)
    {
        await Sender.Send(new UpdateCompanyLinksCommand
        {
            CompanyId = AccountInfo!.Id,
            Links = command.Links
        });

        return NoContent();
    }

    [HttpPost("self/logo")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImageResponse))]
    public async Task<IActionResult> UpdateSelfCompanyLogo(IFormFile? file)
    {
        var result = await Sender.Send(new UpdateCompanyLogoCommand
        {
            CompanyId = AccountInfo!.Id,
            Logo = file
        });

        return Ok(new ImageResponse { Route = result });
    }

    [HttpPost("self/banner")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImageResponse))]
    public async Task<IActionResult> UpdateSelfCompanyBanner(IFormFile? file)
    {
        var result = await Sender.Send(new UpdateCompanyBannerCommand
        {
            CompanyId = AccountInfo!.Id,
            Banner = file
        });

        return Ok(new ImageResponse { Route = result });
    }

    [HttpGet("self/amount-subscribers")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    public async Task<IActionResult> GetAmountStudentSubscribersOfSelfCompany()
    {
        return Ok(await Sender.Send(new GetAmountStudentSubscribersOfCompanyQuery
        {
            CompanyId = AccountInfo!.Id,
            IsSubscriberMustBeVerified = true
        }));
    }

    [HttpGet("self/amount-jobOffers")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    public async Task<IActionResult> GetAmountJobOffersOfSelfCompany()
    {
        return Ok(await Sender.Send(new GetAmountJobOffersOfCompanyQuery
        {
            CompanyId = AccountInfo!.Id
        }));
    }

    [HttpDelete("self")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteSelfCompany()
    {
        return Ok(await Sender.Send(new DeleteCompanyCommand(AccountInfo!.Id)));
    }
}
