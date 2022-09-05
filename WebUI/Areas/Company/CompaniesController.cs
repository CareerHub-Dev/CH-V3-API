using Application.Companies.Commands.DeleteCompany;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;

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
        if (companyId != AccountInfo!.Id)
        {
            return StatusCode(403);
        }

        await Mediator.Send(new DeleteCompanyCommand(companyId));

        return NoContent();
    }
}
