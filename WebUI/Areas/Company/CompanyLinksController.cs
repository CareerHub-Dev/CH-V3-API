using Application.CompanyLinks.Command.CreateCompanyLink;
using Application.CompanyLinks.Command.DeleteCompanyLink;
using Application.CompanyLinks.Command.UpdateCompanyLink;
using Application.CompanyLinks.Queries;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;
using WebUI.Common.Models.Company;

namespace WebUI.Areas.Company;

[Authorize("Company")]
[Route("api/Company/[controller]")]
public class CompanyLinksController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateCompanyLink(CreateCompanyLinkView view)
    {
        return await Mediator.Send(new CreateCompanyLinkCommand
        {
            Name = view.Name,
            Uri = view.Uri,
            CompanyId = AccountInfo!.Id
        });
    }

    [HttpDelete("{companyLinkId}")]
    public async Task<IActionResult> DeleteCompanyLink(Guid companyLinkId)
    {
        if (!await Mediator.Send(new CompanyOwnsCompanyLinkQuery { CompanyLinkId = companyLinkId, CompanyId = AccountInfo!.Id }))
        {
            return Problem(title: "CompanyLink was not found.", statusCode: StatusCodes.Status404NotFound, detail: "Company don't own this company link.");
        }

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

        if (!await Mediator.Send(new CompanyOwnsCompanyLinkQuery { CompanyLinkId = companyLinkId, CompanyId = AccountInfo!.Id }))
        {
            return Problem(title: "CompanyLink was not found.", statusCode: StatusCodes.Status404NotFound, detail: "Company don't own this company link.");
        }

        await Mediator.Send(command);

        return NoContent();
    }
}
