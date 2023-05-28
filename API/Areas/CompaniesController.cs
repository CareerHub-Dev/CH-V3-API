using Application.Common.DTO.Companies;
using Application.Companies.Queries.GetBriefCompanies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Areas;

[Route("api/[controller]")]
public class CompaniesController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BriefCompanyDTO>))]
    public async Task<IActionResult> GetCompanies(
        [FromQuery] string? order,
        [FromQuery] string? search,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Sender.Send(new GetBriefCompaniesWithPagingQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,

            SearchTerm = search ?? string.Empty,

            IsCompanyMustBeVerified = true,

            OrderByExpression = order ?? "Name",
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }
}
