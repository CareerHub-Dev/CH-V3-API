using Application.Companies.Commands.DeleteCompany;
using Application.Companies.Commands.InviteCompany;
using Application.Companies.Commands.UpdateCompany;
using Application.Companies.Commands.UpdateCompanyBanner;
using Application.Companies.Commands.UpdateCompanyLogo;
using Application.Emails.Commands;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;
using WebUI.Common.Extentions;
using WebUI.Common.Models.Company;

namespace WebUI.Controllers;

public class CompaniesController : ApiControllerBase
{
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
    /// Admin Student
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
                        CompanyName = updateCompany.CompanyName,
                        CompanyMotto = updateCompany.CompanyMotto,
                        CompanyDescription = updateCompany.CompanyDescription
                    });
                    break;
                }
            default:
                return StatusCode(403);
        }

        return NoContent();
    }

    /// <summary>
    /// Admin Student
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
    /// Admin Student
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
