using Application.Common.DTO.CompanyLinks;
using Application.CompanyLinks.Command.CreateCompanyLink;
using Application.CompanyLinks.Command.DeleteCompanyLinkOfCompany;
using Application.CompanyLinks.Command.UpdateCompanyLinkOfCompany;
using Application.CompanyLinks.Queries.GetCompanyLink;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;
using WebUI.ViewModels.CompanyLinks;

namespace WebUI.Areas.Company;

[Authorize("Company")]
[Route("api/Company/[controller]")]
public class CompanyLinksController : ApiControllerBase
{
    [HttpGet("{companyLinkId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CompanyLinkDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCompanyLink(Guid companyLinkId)
    {
        return Ok(await Mediator.Send(new GetCompanyLinkOfCompanyQuery
        {
            CompanyLinkId = companyLinkId,
            CompanyId = AccountInfo!.Id,
            IsCompanyMustBeVerified = true
        }));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCompanyLink(CreateCompanyLinkView view)
    {
        var result = await Mediator.Send(new CreateCompanyLinkCommand
        {
            Name = view.Name,
            Uri = view.Uri,
            CompanyId = AccountInfo!.Id
        });

        return CreatedAtAction(nameof(GetCompanyLink), new { companyLinkId = result }, result);
    }

    [HttpDelete("{companyLinkId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCompanyLink(Guid companyLinkId)
    {
        await Mediator.Send(new DeleteCompanyLinkOfCompanyCommand(companyLinkId, AccountInfo!.Id));

        return NoContent();
    }

    [HttpPut("{companyLinkId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateCompanyLink(Guid companyLinkId, UpdateCompanyLinkView view)
    {
        if (companyLinkId != view.CompanyLinkId)
        {
            return BadRequest();
        }

        await Mediator.Send(new UpdateCompanyLinkOfCompanyCommand
        {
            CompanyLinkId = view.CompanyLinkId,
            Name = view.Name,
            Uri = view.Uri,
            CompanyId = AccountInfo!.Id
        });

        return NoContent();
    }
}
