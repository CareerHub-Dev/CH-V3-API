﻿using Application.Companies.Commands.UpdateCompany;
using Application.Companies.Commands.UpdateCompanyBanner;
using Application.Companies.Commands.UpdateCompanyLogo;
using Application.Companies.Queries;
using Application.Companies.Queries.Models;
using Application.CompanyLinks.Queries;
using Microsoft.AspNetCore.Mvc;
using WebUI.Areas.Auth;
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
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Guid?))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateOwnCompanyLogo(IFormFile? file)
    {
        var result = await Mediator.Send(new UpdateCompanyLogoCommand { CompanyId = AccountInfo!.Id, Logo = file });

        return Created(
            Url.ActionLink("GetImage", "Images", new { imageId = result }) ?? "",
            result
        );
    }

    [HttpPost("own/banner")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Guid?))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateOwnCompanyBanner(IFormFile? file)
    {
        var result = await Mediator.Send(new UpdateCompanyBannerCommand { CompanyId = AccountInfo!.Id, Banner = file });

        return Created(
            Url.ActionLink("GetImage", "Images", new { imageId = result }) ?? "",
            result
        );
    }

    [HttpGet("own/companyLinks")]
    public async Task<IEnumerable<CompanyLinkDTO>> GetCompanyLinksOfOwnCompany()
    {
        return await Mediator.Send(new GetCompanyLinksOfCompanyWithFilterQuery
        {
            CompanyId = AccountInfo!.Id,
            IsCompanyVerified = true,
        });
    }

    [HttpGet("own/amountSubscribers")]
    public async Task<int> GetAmountSubscribersOfOwnCompany()
    {
        return await Mediator.Send(new GetAmountSubscribersOfCompanyWithFilterQuery
        {
            CompanyId = AccountInfo!.Id,
            IsVerified = true,
            IsSubscriberVerified = true
        });
    }

    [HttpGet("own/amountJobOffers")]
    public async Task<int> GetAmountJobOffersOfOwnCompany()
    {
        return await Mediator.Send(new GetAmountJobOffersOfCompanyWithFilterQuery
        {
            CompanyId = AccountInfo!.Id,
            IsVerified = true,
        });
    }
}
