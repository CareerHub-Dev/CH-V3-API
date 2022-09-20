using Application.Common.DTO.Tags;
using Application.Tags.Commands.CreateTag;
using Application.Tags.Commands.DeleteTag;
using Application.Tags.Commands.UpdateTag;
using Application.Tags.Queries.GetTag;
using Application.Tags.Queries.GetTags;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebUI.Authorize;

namespace WebUI.Areas.Admin;

[Authorize("Admin", "SuperAdmin")]
[Route("api/Admin/[controller]")]
public class TagsController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<TagDTO>))]
    public async Task<IActionResult> GetTags([FromQuery] GetTagsWithPaginationWithSearchWithFilterQuery query)
    {
        var result = await Mediator.Send(query);

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("{tagId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TagDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTag(Guid tagId)
    {
        return Ok(await Mediator.Send(new GetTagQuery(tagId)));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Guid))]
    public async Task<IActionResult> CreateTag(CreateTagCommand command)
    {
        var result = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetTag), new { tagId = result }, result);
    }

    [HttpPut("{tagId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateTag(Guid tagId, UpdateTagCommand command)
    {
        if (tagId != command.TagId)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{tagId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTag(Guid tagId)
    {
        await Mediator.Send(new DeleteTagCommand(tagId));

        return NoContent();
    }
}
