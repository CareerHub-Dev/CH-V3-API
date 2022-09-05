using Application.Companies.Query;
using Application.CompanyLinks.Query;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebUI.Authorize;
using WebUI.Common.Models;
using WebUI.Common.Models.Company;
using WebUI.Common.Models.CompanyLink;

namespace WebUI.Areas.Student;

[Authorize("Student")]
[Route("api/Student/[controller]")]
public class CompaniesController : ApiControllerBase
{
    /// <remarks>   
    /// Student
    /// 
    ///     get all CompanyLinks Of Verified Company
    ///
    /// </remarks>
    [HttpGet("{companyId}/companyLinks")]
    public async Task<IEnumerable<CompanyLinkBriefResponse>> GetCompanyLinksOfCompany(Guid companyId)
    {
        var result = await Mediator.Send(new GetCompanyLinkBriefsOfCompanyWithFilterQuery
        {
            CompanyId = companyId,
            IsCompanyVerified = true
        });

        return result.Select(x => new CompanyLinkBriefResponse(x));
    }
}
