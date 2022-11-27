using API.Authorize;
using Application.Common.DTO.Tags;
using Application.Tags.Queries.GetBriefTag;
using Application.Tags.Queries.GetBriefTags;
using Microsoft.AspNetCore.Mvc;

namespace API.Areas.Auth;

[Authorize]
[Route("api/Auth/[controller]")]
public class TagsController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BriefTagDTO>))]
    public async Task<IActionResult> GetTags(
        [FromQuery] string? search)
    {
        return Ok(await Sender.Send(new GetBriefTagsQuery
        {
            SearchTerm = search ?? string.Empty,
        }));
    }

    [HttpGet("{tagId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BriefTagDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTag(Guid tagId)
    {
        return Ok(await Sender.Send(new GetBriefTagQuery(tagId)));
    }
}
