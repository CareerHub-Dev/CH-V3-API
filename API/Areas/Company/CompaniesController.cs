using Application.Common.DTO.Companies;
using Application.Companies.Commands.DeleteCompany;
using Application.Companies.Commands.UpdateCompanyBanner;
using Application.Companies.Commands.UpdateCompanyDetail;
using Application.Companies.Commands.UpdateCompanyLogo;
using Application.Companies.Queries.GetAmount;
using Application.Companies.Queries.GetCompany;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using API.Authorize;
using API.DTO.Requests.Companies;

namespace API.Areas.Company;

[Authorize("Company")]
[Route("api/Company/[controller]")]
public class CompaniesController : ApiControllerBase
{
    [HttpGet("self")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DetailedCompanyDTO))]
    public async Task<IActionResult> GetSelfCompany()
    {
        return Ok(await Mediator.Send(new GetDetailedCompanyWithFilterQuery
        {
            CompanyId = AccountInfo!.Id
        }));
    }

    [HttpPut("self/detail")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateSelfCompanyDetailAccount(UpdateOwnCompanyDetailRequest view)
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
    public async Task<IActionResult> UpdateSelfCompanyLogo(IFormFile? file)
    {
        var result = await Mediator.Send(new UpdateCompanyLogoCommand { CompanyId = AccountInfo!.Id, Logo = file });

        return Ok(result);
    }

    [HttpPost("self/banner")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid?))]
    public async Task<IActionResult> UpdateSelfCompanyBanner(IFormFile? file)
    {
        var result = await Mediator.Send(new UpdateCompanyBannerCommand { CompanyId = AccountInfo!.Id, Banner = file });

        return Ok(result);
    }

    [HttpGet("self/amount-subscribers")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    public async Task<IActionResult> GetAmountStudentSubscribersOfSelfCompany()
    {
        return Ok(await Mediator.Send(new GetAmountStudentSubscribersOfCompanyWithFilterQuery
        {
            CompanyId = AccountInfo!.Id,
            IsSubscriberMustBeVerified = true,
            SubscriberMustHaveActivationStatus = ActivationStatus.Active
        }));
    }

    [HttpGet("self/amount-jobOffers")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    public async Task<IActionResult> GetAmountJobOffersOfSelfCompany()
    {
        return Ok(await Mediator.Send(new GetAmountJobOffersOfCompanyWithFilterQuery
        {
            CompanyId = AccountInfo!.Id
        }));
    }

    [HttpDelete("self")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteSelfCompany()
    {
        return Ok(await Mediator.Send(new DeleteCompanyCommand(AccountInfo!.Id)));
    }
}
