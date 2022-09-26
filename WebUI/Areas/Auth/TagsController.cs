using Application.Common.DTO.Tags;
using Application.Tags.Queries.GetTag;
using Application.Tags.Queries.GetTags;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;

namespace WebUI.Areas.Auth;

[Authorize]
[Route("api/Auth/[controller]")]
public class TagsController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<BriefTagDTO>))]
    public async Task<IActionResult> GetTags([FromQuery] GetBriefTagsWithSearchQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpGet("{tagId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BriefTagDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTag(Guid tagId)
    {
        return Ok(await Mediator.Send(new GetBriefTagQuery(tagId)));
    }
}
