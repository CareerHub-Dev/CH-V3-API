using Application.Common.DTO.CompanyLinks;
using Application.CompanyLinks.Command.CreateCompanyLink;
using Application.CompanyLinks.Command.DeleteCompanyLinkOfCompany;
using Application.CompanyLinks.Command.UpdateCompanyLinkOfCompany;
using Application.CompanyLinks.Queries.GetCompanyLink;
using Application.CompanyLinks.Queries.GetCompanyLinks;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;
using WebUI.ViewModels.CompanyLinks;

namespace WebUI.Areas.Company;

[Authorize("Company")]
[Route("api/Company/self/[controller]")]
public class CompanyLinksController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CompanyLinkDTO>))]
    public async Task<IActionResult> GetCompanyLinksOfSelfCompany()
    {
        return Ok(await Mediator.Send(new GetCompanyLinksOfCompanyWithFilterQuery
        {
            CompanyId = AccountInfo!.Id,
        }));
    }

    [HttpGet("{companyLinkId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CompanyLinkDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCompanyLinkOfSelfCompany(Guid companyLinkId)
    {
        return Ok(await Mediator.Send(new GetCompanyLinkOfCompanyWithFilterQuery
        {
            CompanyLinkId = companyLinkId,
            CompanyId = AccountInfo!.Id,
        }));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCompanyLinkForSelfCompany(CreateCompanyLinkView view)
    {
        var result = await Mediator.Send(new CreateCompanyLinkCommand
        {
            Title = view.Title,
            Uri = view.Uri,
            CompanyId = AccountInfo!.Id
        });

        return Ok(result);
    }

    [HttpDelete("{companyLinkId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCompanyLinkOfSelfCompany(Guid companyLinkId)
    {
        await Mediator.Send(new DeleteCompanyLinkOfCompanyCommand(companyLinkId, AccountInfo!.Id));

        return NoContent();
    }

    [HttpPut("{companyLinkId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateCompanyLinkOfSelfCompany(Guid companyLinkId, UpdateCompanyLinkView view)
    {
        if (companyLinkId != view.CompanyLinkId)
        {
            return BadRequest();
        }

        await Mediator.Send(new UpdateCompanyLinkOfCompanyCommand
        {
            CompanyLinkId = view.CompanyLinkId,
            Title = view.Title,
            Uri = view.Uri,
            CompanyId = AccountInfo!.Id
        });

        return NoContent();
    }
}
