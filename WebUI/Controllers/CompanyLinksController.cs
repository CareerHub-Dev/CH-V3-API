using Application.CompanyLinks.Command.CompanyOwnsCompanyLink;
using Application.CompanyLinks.Command.CreateCompanyLink;
using Application.CompanyLinks.Command.DeleteCompanyLink;
using Application.CompanyLinks.Command.UpdateCompanyLink;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;
using WebUI.Common.Models.CompanyLink;

namespace WebUI.Controllers;

public class CompanyLinksController : ApiControllerBase
{
    /// <summary>
    /// Company
    /// </summary>
    [HttpPost]
    [Authorize("Company")]
    public async Task<Guid> CreateCompanyLink(CreateCompanyLinkRequest createCompanyLink)
    {
        return await Mediator.Send(new CreateCompanyLinkCommand { Name = createCompanyLink.Name, Uri = createCompanyLink.Uri, CompanyId = AccountInfo!.Id });
    }

    /// <summary>
    /// Company
    /// </summary>
    [HttpPut("{companyLinkId}")]
    [Authorize("Company")]
    public async Task<IActionResult> UpdateCompanyLink(Guid companyLinkId, UpdateCompanyLinkRequest updateCompanyLink)
    {
        if (!await Mediator.Send(new CompanyOwnsCompanyLinkCommand { CompanyId = AccountInfo!.Id, CompanyLinkId = companyLinkId }))
        {
            return Problem(title: "Not Found", statusCode: StatusCodes.Status404NotFound, detail: "CompanyLink is not found");
        }

        await Mediator.Send(new UpdateCompanyLinkCommand { Name = updateCompanyLink.Name, Uri = updateCompanyLink.Uri, CompanyLinkId = companyLinkId });
        return NoContent();
    }

    /// <summary>
    /// Company
    /// </summary>
    /// /// <remarks>
    /// Admin:
    /// 
    ///     delete any companyLink
    ///
    /// Company:
    /// 
    ///     delete own companyLink
    ///
    /// </remarks>
    [HttpDelete("{companyLinkId}")]
    [Authorize("Company", "Admin")]
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
