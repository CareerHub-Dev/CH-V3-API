using Application.Common.DTO.Tags;
using Application.Tags.Commands.CreateTag;
using Application.Tags.Queries.GetTag;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;
using WebUI.ViewModels.Tags;

namespace WebUI.Areas.Company;

[Authorize("Company")]
[Route("api/Company/[controller]")]
public class TagsController : ApiControllerBase
{
    [HttpGet("{tagId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BriefTagDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTag(Guid tagId)
    {
        return Ok(await Mediator.Send(new GetBriefTagQuery(tagId)));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Guid))]
    public async Task<IActionResult> CreateTag(CreateTagView view)
    {
        var result = await Mediator.Send(new CreateTagCommand { Name = view.Name, IsAccepted = false });

        return CreatedAtAction(nameof(GetTag), new { tagId = result }, result);
    }
}
