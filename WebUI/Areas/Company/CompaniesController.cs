using Application.Companies.Commands.UpdateCompany;
using Application.Companies.Commands.UpdateCompanyBanner;
using Application.Companies.Commands.UpdateCompanyLogo;
using Application.Companies.Queries;
using Application.Companies.Queries.Models;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;
using WebUI.Common.Models.Company;

namespace WebUI.Areas.Company;

[Authorize("Company")]
[Route("api/Company/[controller]")]
public class CompaniesController : ApiControllerBase
{
    [HttpGet("own")]
    public async Task<CompanyDetailedDTO> GetCompany()
    {
        return await Mediator.Send(new GetCompanyDetailedWithFilterQuery
        {
            CompanyId = AccountInfo!.Id,
            IsVerified = true
        });
    }

    [HttpPut("own")]
    public async Task<IActionResult> UpdateOwnCompanyAccount(UpdateOwnCompanyAccountView view)
    {
        await Mediator.Send(new UpdateCompanyCommand
        {
            CompanyId = AccountInfo!.Id,
            Motto = view.Motto,
            Description = view.Description,
            Name = view.Name,
        });

        return NoContent();
    }

    [HttpPost("own/logo")]
    public async Task<Guid?> UpdateOwnCompanyLogo(IFormFile? file)
    {
        return await Mediator.Send(new UpdateCompanyLogoCommand { CompanyId = AccountInfo!.Id, Logo = file });
    }

    [HttpPost("own/banner")]
    public async Task<Guid?> UpdateOwnCompanyBanner(IFormFile? file)
    {
        return await Mediator.Send(new UpdateCompanyBannerCommand { CompanyId = AccountInfo!.Id, Banner = file });
    }
}
