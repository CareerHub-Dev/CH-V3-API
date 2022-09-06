using Application.CompanyLinks.Command.CreateCompanyLink;
using Application.CompanyLinks.Command.DeleteCompanyLink;
using Application.CompanyLinks.Command.UpdateCompanyLink;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;

namespace WebUI.Areas.Admin;

[Authorize("Admin", "SuperAdmin")]
[Route("api/Admin/[controller]")]
public class CompanyLinksController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateCompanyLink(CreateCompanyLinkCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpDelete("{companyLinkId}")]
    public async Task<IActionResult> DeleteCompanyLink(Guid companyLinkId)
    {
        await Mediator.Send(new DeleteCompanyLinkCommand(companyLinkId));

        return NoContent();
    }

    [HttpPut("{companyLinkId}")]
    public async Task<IActionResult> UpdateCompanyLink(Guid companyLinkId, UpdateCompanyLinkCommand command)
    {
        if (companyLinkId != command.CompanyLinkId)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }
}
