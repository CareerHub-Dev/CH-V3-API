using Application.CompanyLinks.Command.CompanyOwnsCompanyLink;
using Application.CompanyLinks.Command.CreateCompanyLink;
using Application.CompanyLinks.Command.DeleteCompanyLink;
using Application.CompanyLinks.Command.UpdateCompanyLink;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;
using WebUI.Common.Models.CompanyLink;

namespace WebUI.Areas.Company;

[Authorize("Company")]
[Route("api/Company/[controller]")]
public class CompanyLinksController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateCompanyLink(CreateCompanyLinkRequest createCompanyLink)
    {
        if(createCompanyLink.CompanyId != AccountInfo!.Id)
        {
            return StatusCode(403);
        }

        return await Mediator.Send(new CreateCompanyLinkCommand { Name = createCompanyLink.Name, Uri = createCompanyLink.Uri, CompanyId = createCompanyLink.CompanyId });
    }

    /// <remarks>
    /// Company:
    /// 
    ///     update own companyLink
    ///
    /// </remarks>
    [HttpPut("{companyLinkId}")]
    public async Task<IActionResult> UpdateCompanyLink(Guid companyLinkId, UpdateCompanyLinkRequest updateCompanyLink)
    {
        if (!await Mediator.Send(new CompanyOwnsCompanyLinkCommand { CompanyId = AccountInfo!.Id, CompanyLinkId = companyLinkId }))
        {
            return Problem(title: "Not Found", statusCode: StatusCodes.Status404NotFound, detail: "CompanyLink is not found");
        }

        await Mediator.Send(new UpdateCompanyLinkCommand { Name = updateCompanyLink.Name, Uri = updateCompanyLink.Uri, CompanyLinkId = companyLinkId });
        return NoContent();
    }

    /// <remarks>
    /// Company:
    /// 
    ///     delete own companyLink
    ///
    /// </remarks>
    [HttpDelete("{companyLinkId}")]
    public async Task<IActionResult> DeleteCompanyLink(Guid companyLinkId)
    {
        if (AccountInfo!.Role != "Admin" && !await Mediator.Send(new CompanyOwnsCompanyLinkCommand { CompanyId = AccountInfo!.Id, CompanyLinkId = companyLinkId }))
        {
            return Problem(title: "Not Found", statusCode: StatusCodes.Status404NotFound, detail: "CompanyLink is not found");
        }

        await Mediator.Send(new DeleteCompanyLinkCommand(companyLinkId));
        return NoContent();
    }
}
