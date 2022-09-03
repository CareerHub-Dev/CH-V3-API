﻿using Application.Companies.Commands.DeleteCompany;
using Application.Companies.Commands.InviteCompany;
using Application.Companies.Commands.UpdateCompany;
using Application.Companies.Commands.UpdateCompanyBanner;
using Application.Companies.Commands.UpdateCompanyLogo;
using Application.CompanyLinks.Query;
using Application.Emails.Commands;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;
using WebUI.Common.Extentions;
using WebUI.Common.Models.Company;
using WebUI.Common.Models.CompanyLink;

namespace WebUI.Areas.Admin;

[Authorize("Admin")]
[Route("api/Admin/[controller]")]
public class CompaniesController : ApiControllerBase
{
    /// <remarks>
    /// Admin:
    /// 
    ///     Create company (sends an e-mail under the hood)
    ///
    /// </remarks>
    [HttpPost("invite")]
    public async Task<ActionResult<Guid>> InviteCompany(InviteCompanyRequest inviteCompany)
    {
        return await Mediator.Send(new InviteCompanyCommand { Email = inviteCompany.Email });
    }

    /// <remarks>
    /// Admin:
    /// 
    ///     sends an e-mail
    ///
    /// </remarks>
    [HttpPost("send-invite-email")]
    public async Task<IActionResult> SendInviteCompanyEmail(SendInviteCompanyEmailRequest sendInviteCompanyEmail)
    {
        await Mediator.Send(new SendInviteCompanyEmailCommand(sendInviteCompanyEmail.CompanyId));
        return Ok();
    }

    [HttpDelete("{companyId}")]
    public async Task<IActionResult> DeleteCompany(Guid companyId)
    {
        await Mediator.Send(new DeleteCompanyCommand(companyId));

        return NoContent();
    }

    [HttpPut("{companyId}")]
    public async Task<IActionResult> UpdateCompany(Guid companyId, UpdateCompanyRequest updateCompany)
    {
        await Mediator.Send(new UpdateCompanyCommand
        {
            CompanyId = companyId,
            Name = updateCompany.Name,
            Motto = updateCompany.Motto,
            Description = updateCompany.Description
        });

        return NoContent();
    }

    [HttpPut("{companyId}/logo")]
    public async Task<ActionResult<Guid?>> UpdateCompanyLogo(Guid companyId, [FromForm] UpdateCompanyLogoRequest updateCompanyLogo)
    {
        return await Mediator.Send(new UpdateCompanyLogoCommand
        {
            CompanyId = companyId,
            Logo = updateCompanyLogo.LogoFile is IFormFile logo ? await logo.ToCreateImageAsync() : null,
        });
    }

    [HttpPut("{companyId}/banner")]
    public async Task<ActionResult<Guid?>> UpdateCompanyBanner(Guid companyId, [FromForm] UpdateCompanyBannerRequest updateCompanyBanner)
    {
        return await Mediator.Send(new UpdateCompanyBannerCommand
        {
            CompanyId = companyId,
            Banner = updateCompanyBanner.BannerFile is IFormFile banner ? await banner.ToCreateImageAsync() : null,
        });
    }

    /// <remarks>   
    /// Company
    /// 
    ///     get all CompanyLinks Of Company
    ///
    /// </remarks>
    [HttpGet("{companyId}/companyLinks")]
    public async Task<IEnumerable<CompanyLinkResponse>> GetCompanyLinksOfCompany(Guid companyId)
    {
        var result = await Mediator.Send(new GetCompanyLinksOfCompanyWithFilterQuery
        {
            CompanyId = companyId
        });

        return result.Select(x => new CompanyLinkResponse(x));
    }
}