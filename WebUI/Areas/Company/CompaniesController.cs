using Application.Companies.Commands.DeleteCompany;
using Application.Companies.Commands.UpdateCompany;
using Application.Companies.Commands.UpdateCompanyBanner;
using Application.Companies.Commands.UpdateCompanyLogo;
using Application.Companies.Query;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebUI.Authorize;
using WebUI.Common.Extentions;
using WebUI.Common.Models;
using WebUI.Common.Models.Company;

namespace WebUI.Areas.Company;

[Authorize("Company")]
[Route("api/Company/[controller]")]
public class CompaniesController : ApiControllerBase
{
    /// <remarks>   
    /// Company
    /// 
    ///     get all Verified Brief companies
    ///
    /// </remarks>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CompanyBriefResponse>>> GetCompanies(
        [FromQuery] PaginationParameters paginationParameters,
        [FromQuery] SearchParameter searchParameter)
    {
        var result = await Mediator.Send(new GetCompanyBriefsWithPaginationWithSearchWithFilterQuery
        {
            PageNumber = paginationParameters.PageNumber,
            PageSize = paginationParameters.PageSize,
            SearchTerm = searchParameter.SearchTerm,
            WithoutCompanyId = AccountInfo!.Id,
            IsVerified = true
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result.Select(x => new CompanyBriefResponse(x)));
    }

    /// <remarks>   
    /// Company
    /// 
    ///     get Verified Detailed company
    ///
    /// </remarks>
    [HttpGet("{companyId}")]
    public async Task<CompanyDetailedResponse> GetCompany(Guid companyId)
    {
        var result = await Mediator.Send(new GetCompanyDetailedWithFilterQuery
        {
            CompanyId = companyId,
            IsVerified = true
        });

        return new CompanyDetailedResponse(result);
    }

    /// <remarks>
    /// Company:
    /// 
    ///     delete own account
    ///
    /// </remarks>
    [HttpDelete("{companyId}")]
    public async Task<IActionResult> DeleteCompany(Guid companyId)
    {
        if(companyId != AccountInfo!.Id)
        {
            return StatusCode(403);
        }

        await Mediator.Send(new DeleteCompanyCommand(companyId));

        return NoContent();
    }

    /// <remarks>  
    /// Company:
    /// 
    ///     update own account
    ///
    /// </remarks>
    [HttpPut("{companyId}")]
    public async Task<IActionResult> UpdateCompany(Guid companyId, UpdateCompanyRequest updateCompany)
    {
        if (companyId != AccountInfo!.Id)
        {
            return StatusCode(403);
        }

        await Mediator.Send(new UpdateCompanyCommand
        {
            CompanyId = companyId,
            Name = updateCompany.Name,
            Motto = updateCompany.Motto,
            Description = updateCompany.Description
        });

        return NoContent();
    }

    /// <remarks>
    /// Company:
    /// 
    ///     update own logo
    ///
    /// </remarks>
    [HttpPut("{companyId}/logo")]
    [Authorize("Admin", "Company")]
    public async Task<ActionResult<Guid?>> UpdateCompanyLogo(Guid companyId, [FromForm] UpdateCompanyLogoRequest updateCompanyLogo)
    {
        if (companyId != AccountInfo!.Id)
        {
            return StatusCode(403);
        }

        return await Mediator.Send(new UpdateCompanyLogoCommand
        {
            CompanyId = companyId,
            Logo = updateCompanyLogo.LogoFile is IFormFile logo ? await logo.ToCreateImageAsync() : null,
        });
    }

    /// <remarks> 
    /// Company:
    /// 
    ///     update own banner
    ///
    /// </remarks>
    [HttpPut("{companyId}/banner")]
    public async Task<ActionResult<Guid?>> UpdateCompanyBanner(Guid companyId, [FromForm] UpdateCompanyBannerRequest updateCompanyBanner)
    {
        if (companyId != AccountInfo!.Id)
        {
            return StatusCode(403);
        }

        return await Mediator.Send(new UpdateCompanyBannerCommand
        {
            CompanyId = companyId,
            Banner = updateCompanyBanner.BannerFile is IFormFile banner ? await banner.ToCreateImageAsync() : null,
        });
    }
}
