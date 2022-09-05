using Application.Companies.Commands.DeleteCompany;
using Application.Companies.Commands.UpdateCompany;
using Application.Companies.Commands.UpdateCompanyBanner;
using Application.Companies.Commands.UpdateCompanyLogo;
using Application.Companies.Query;
using Application.CompanyLinks.Query;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebUI.Authorize;
using WebUI.Common.Extentions;
using WebUI.Common.Models;
using WebUI.Common.Models.Company;
using WebUI.Common.Models.CompanyLink;

namespace WebUI.Areas.Company;

[Authorize("Company")]
[Route("api/Company/[controller]")]
public class CompaniesController : ApiControllerBase
{
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
    /// Company
    /// 
    ///     get all CompanyLinks Of Verified Company or get all own CompanyLinks (different models)
    ///
    /// </remarks>
    [HttpGet("{companyId}/companyLinks")]
    public async Task<IActionResult> GetCompanyLinksOfCompany(Guid companyId)
    {
        if (companyId == AccountInfo!.Id)
        {
            var resultOwn = await Mediator.Send(new GetCompanyLinkDetailedsOfCompanyWithFilterQuery
            {
                CompanyId = companyId
            });

            return Ok(resultOwn.Select(x => new CompanyLinkDetailedResponse(x)));
        }

        var result = await Mediator.Send(new GetCompanyLinkBriefsOfCompanyWithFilterQuery
        {
            CompanyId = companyId,
            IsCompanyVerified = true
        });

        return Ok(result.Select(x => new CompanyLinkBriefResponse(x)));
    }
}
