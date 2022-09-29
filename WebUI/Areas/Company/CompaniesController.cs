using Application.Common.DTO.Companies;
using Application.Companies.Commands.UpdateCompanyBanner;
using Application.Companies.Commands.UpdateCompanyDetail;
using Application.Companies.Commands.UpdateCompanyLogo;
using Application.Companies.Queries.GetAmount;
using Application.Companies.Queries.GetCompany;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;
using WebUI.ViewModels.Companies;

namespace WebUI.Areas.Company;

[Authorize("Company")]
[Route("api/Company/[controller]")]
public class CompaniesController : ApiControllerBase
{
    [HttpGet("self")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DetailedCompanyDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSelfCompany()
    {
        return Ok(await Mediator.Send(new GetDetailedCompanyWithFilterQuery
        {
            CompanyId = AccountInfo!.Id
        }));
    }

    [HttpPut("self/detail")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSelfCompanyDetailAccount(UpdateOwnCompanyDetailView view)
    {
        await Mediator.Send(new UpdateCompanyDetailCommand
        {
            CompanyId = AccountInfo!.Id,
            Motto = view.Motto,
            Description = view.Description,
            Name = view.Name,
        });

        return NoContent();
    }

    [HttpPost("self/logo")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid?))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSelfCompanyLogo(IFormFile? file)
    {
        var result = await Mediator.Send(new UpdateCompanyLogoCommand { CompanyId = AccountInfo!.Id, Logo = file });

        return Ok(result);
    }

    [HttpPost("self/banner")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid?))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSelfCompanyBanner(IFormFile? file)
    {
        var result = await Mediator.Send(new UpdateCompanyBannerCommand { CompanyId = AccountInfo!.Id, Banner = file });

        return Ok(result);
    }

    [HttpGet("self/amount-subscribers")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountSubscribersOfSelfCompany()
    {
        return Ok(await Mediator.Send(new GetAmountSubscribersOfCompanyWithFilterQuery
        {
            CompanyId = AccountInfo!.Id,
            IsSubscriberMustBeVerified = true
        }));
    }

    [HttpGet("self/amount-jobOffers")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmountJobOffersOfSelfCompany()
    {
        return Ok(await Mediator.Send(new GetAmountJobOffersOfCompanyWithFilterQuery
        {
            CompanyId = AccountInfo!.Id
        }));
    }

}
