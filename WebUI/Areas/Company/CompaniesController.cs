using Application.Companies.Queries;
using Application.Companies.Queries.Models;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;

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
}
