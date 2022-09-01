using Application.Companies.Commands.DeleteCompany;
using Application.Companies.Commands.InviteCompany;
using Application.Companies.Commands.UpdateCompany;
using Application.Companies.Commands.UpdateCompanyBanner;
using Application.Companies.Commands.UpdateCompanyLogo;
using Application.Companies.Query;
using Application.Emails.Commands;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebUI.Authorize;
using WebUI.Common.Extentions;
using WebUI.Common.Models;
using WebUI.Common.Models.Company;

namespace WebUI.Controllers;

public class CompaniesController : ApiControllerBase
{
    /// <summary>
    /// Student Company
    /// </summary>
    /// <remarks>
    /// Student
    /// 
    ///     get all Verified companies
    ///     
    /// Company
    /// 
    ///     get all Verified companies
    ///
    /// </remarks>
    [HttpGet]
    [Authorize("Student", "Company")]
    public async Task<IActionResult> GetCompanies(
        [FromQuery] PaginationParameters paginationParameters,
        [FromQuery] SearchParameter searchParameter)
    {
        switch (AccountInfo!.Role)
        {
            case "Company":
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
            case "Student":
                {
                    var result = await Mediator.Send(new GetCompanyBriefsWithPaginationWithSearchWithFilterQuery
                    {
                        PageNumber = paginationParameters.PageNumber,
                        PageSize = paginationParameters.PageSize,
                        SearchTerm = searchParameter.SearchTerm,
                        IsVerified = true
                    });

                    Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

                    return Ok(result.Select(x => new CompanyBriefResponse(x)));
                }
            default:
                return StatusCode(403);
        }
    }

    /// <summary>
    /// Admin
    /// </summary>
    /// <remarks>
    /// Admin:
    /// 
    ///     Create company (sends an e-mail under the hood)
    ///
    /// </remarks>
    [HttpPost("invite")]
    [Authorize("Admin")]
    public async Task<ActionResult<Guid>> InviteCompany(InviteCompanyRequest inviteCompany)
    {
        return await Mediator.Send(new InviteCompanyCommand { Email = inviteCompany.Email });
    }

    /// <summary>
    /// Admin
    /// </summary>
    /// <remarks>
    /// Admin:
    /// 
    ///     sends an e-mail
    ///
    /// </remarks>
    [HttpPost("send-invite-email")]
    [Authorize("Admin")]
    public async Task<IActionResult> SendInviteCompanyEmail(SendInviteCompanyEmailRequest sendInviteCompanyEmail)
    {
        await Mediator.Send(new SendInviteCompanyEmailCommand(sendInviteCompanyEmail.CompanyId));
        return Ok();
    }

    /// <summary>
    /// Admin Company
    /// </summary>
    /// <remarks>
    /// Admin:
    /// 
    ///     delete any company account
    ///
    /// Company:
    /// 
    ///     delete own account
    ///
    /// </remarks>
    [HttpDelete("{companyId}")]
    [Authorize("Admin", "Company")]
    public async Task<IActionResult> DeleteCompany(Guid companyId)
    {
        switch (AccountInfo!.Role)
        {
            case "Admin":
            case "Company" when companyId == AccountInfo.Id:
                await Mediator.Send(new DeleteCompanyCommand(companyId));
                break;
            default:
                return StatusCode(403);
        }

        return NoContent();
    }

    /// <summary>
    /// Admin Company
    /// </summary>
    /// <remarks>
    /// Admin:
    /// 
    ///     update any company
    ///     
    /// Company:
    /// 
    ///     update own account
    ///
    /// </remarks>
    [HttpPut("{companyId}")]
    [Authorize("Admin", "Company")]
    public async Task<IActionResult> UpdateCompany(Guid companyId, UpdateCompanyRequest updateCompany)
    {
        switch (AccountInfo!.Role)
        {
            case "Admin":
            case "Company" when companyId == AccountInfo.Id:
                {
                    await Mediator.Send(new UpdateCompanyCommand
                    {
                        CompanyId = companyId,
                        Name = updateCompany.Name,
                        Motto = updateCompany.Motto,
                        Description = updateCompany.Description
                    });
                    break;
                }
            default:
                return StatusCode(403);
        }

        return NoContent();
    }

    /// <summary>
    /// Admin Company
    /// </summary>
    /// <remarks>
    /// Admin:
    /// 
    ///     update any company logo
    ///     
    /// Company:
    /// 
    ///     update own logo
    ///
    /// </remarks>
    [HttpPut("{companyId}/logo")]
    [Authorize("Admin", "Company")]
    public async Task<ActionResult<Guid?>> UpdateCompanyLogo(Guid companyId, [FromForm] UpdateCompanyLogoRequest updateCompanyLogo)
    {
        switch (AccountInfo!.Role)
        {
            case "Admin":
            case "Company" when companyId == AccountInfo.Id:
                {
                    return await Mediator.Send(new UpdateCompanyLogoCommand
                    {
                        CompanyId = companyId,
                        Logo = updateCompanyLogo.LogoFile is IFormFile logo ? await logo.ToCreateImageAsync() : null,
                    });
                }
            default:
                return StatusCode(403);
        }
    }

    /// <summary>
    /// Admin Company
    /// </summary>
    /// <remarks>
    /// Admin:
    /// 
    ///     update any company banner
    ///     
    /// Company:
    /// 
    ///     update own banner
    ///
    /// </remarks>
    [HttpPut("{companyId}/banner")]
    [Authorize("Admin", "Company")]
    public async Task<ActionResult<Guid?>> UpdateCompanyBanner(Guid companyId, [FromForm] UpdateCompanyBannerRequest updateCompanyBanner)
    {
        switch (AccountInfo!.Role)
        {
            case "Admin":
            case "Company" when companyId == AccountInfo.Id:
                {
                    return await Mediator.Send(new UpdateCompanyBannerCommand
                    {
                        CompanyId = companyId,
                        Banner = updateCompanyBanner.BannerFile is IFormFile banner ? await banner.ToCreateImageAsync() : null,
                    });
                }
            default:
                return StatusCode(403);
        }
    }
}
