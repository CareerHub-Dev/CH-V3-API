using Application.Companies.Commands.DeleteCompany;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;

namespace WebUI.Areas.Company;

[Authorize("Company")]
[Route("api/Company/[controller]")]
public class CompaniesController : ApiControllerBase
{
    
}
