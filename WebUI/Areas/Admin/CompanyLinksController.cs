using Application.CompanyLinks.Command.CreateCompanyLink;
using Application.CompanyLinks.Command.DeleteCompanyLink;
using Application.CompanyLinks.Command.UpdateCompanyLink;
using Application.CompanyLinks.Queries;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;

namespace WebUI.Areas.Admin;

[Authorize("Admin", "SuperAdmin")]
[Route("api/Admin/[controller]")]
public class CompanyLinksController : ApiControllerBase
{
    [HttpGet("{companyLinkId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CompanyLinkDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCompanyLink(Guid companyLinkId)
    {
        return Ok(await Mediator.Send(new GetCompanyLinkQuery(companyLinkId)));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCompanyLink(CreateCompanyLinkCommand command)
    {
        var result = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetCompanyLink), new { companyLinkId = result }, result);
    }

    [HttpDelete("{companyLinkId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCompanyLink(Guid companyLinkId)
    {
        await Mediator.Send(new DeleteCompanyLinkCommand(companyLinkId));

        return NoContent();
    }

    [HttpPut("{companyLinkId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
