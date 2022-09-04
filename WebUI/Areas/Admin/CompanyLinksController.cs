using Application.CompanyLinks.Command.CreateCompanyLink;
using Application.CompanyLinks.Command.DeleteCompanyLink;
using Application.CompanyLinks.Command.UpdateCompanyLink;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;
using WebUI.Common.Models.CompanyLink;

namespace WebUI.Areas.Admin;

[Authorize("Admin", "SuperAdmin")]
[Route("api/Admin/[controller]")]
public class CompanyLinksController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateCompanyLink(CreateCompanyLinkRequest createCompanyLink)
    {
        return await Mediator.Send(new CreateCompanyLinkCommand { Name = createCompanyLink.Name, Uri = createCompanyLink.Uri, CompanyId = createCompanyLink.CompanyId });
    }

    [HttpPut("{companyLinkId}")]
    public async Task<IActionResult> UpdateCompanyLink(Guid companyLinkId, UpdateCompanyLinkRequest updateCompanyLink)
    {
        await Mediator.Send(new UpdateCompanyLinkCommand { Name = updateCompanyLink.Name, Uri = updateCompanyLink.Uri, CompanyLinkId = companyLinkId });
        return NoContent();
    }

    [HttpDelete("{companyLinkId}")]
    public async Task<IActionResult> DeleteCompanyLink(Guid companyLinkId)
    {
        await Mediator.Send(new DeleteCompanyLinkCommand(companyLinkId));
        return NoContent();
    }
}
