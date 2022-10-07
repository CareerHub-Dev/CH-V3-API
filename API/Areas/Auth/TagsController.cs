using Application.Common.DTO.Tags;
using Application.Tags.Queries.GetTag;
using Application.Tags.Queries.GetTags;
using Microsoft.AspNetCore.Mvc;
using API.Authorize;

namespace API.Areas.Auth;

[Authorize]
[Route("api/Auth/[controller]")]
public class TagsController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BriefTagDTO>))]
    public async Task<IActionResult> GetTags(
        [FromQuery] string? searchTerm)
    {
        return Ok(await Mediator.Send(new GetBriefTagsWithSearchQuery
        {
            SearchTerm = searchTerm ?? string.Empty,
        }));
    }

    [HttpGet("{tagId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BriefTagDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTag(Guid tagId)
    {
        return Ok(await Mediator.Send(new GetBriefTagQuery(tagId)));
    }
}
