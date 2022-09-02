using Application.Companies.Query;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebUI.Authorize;
using WebUI.Common.Models;
using WebUI.Common.Models.Company;

namespace WebUI.Areas.Student;

[Authorize("Student")]
[Route("api/Student/[controller]")]
public class CompaniesController : ApiControllerBase
{
    /// <remarks>   
    /// Student
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
            IsVerified = true
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result.Select(x => new CompanyBriefResponse(x)));
    }

    /// <remarks>   
    /// Student
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
}
