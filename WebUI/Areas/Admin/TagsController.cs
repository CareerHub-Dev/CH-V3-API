using Application.Tags.Commands.CreateTag;
using Application.Tags.Commands.DeleteTag;
using Application.Tags.Commands.UpdateTag;
using Application.Tags.Queries;
using Application.Tags.Queries.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebUI.Authorize;
using WebUI.Common.Models.Tag;

namespace WebUI.Areas.Admin;

[Authorize("Admin")]
[Route("api/Admin/[controller]")]
public class TagsController : ApiControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<TagDTO>> GetTags([FromQuery] GetTagsWithPaginationWithSearchWithFilterQuery query)
    {
        var result = await Mediator.Send(query);

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return result;
    }

    [HttpDelete("{tagId}")]
    public async Task<IActionResult> DeleteTag(Guid tagId)
    {
        await Mediator.Send(new DeleteTagCommand(tagId));

        return NoContent();
    }

    [HttpPost]
    public async Task<Guid> CreateTag(CreateTagView view)
    {
        return await Mediator.Send(new CreateTagCommand { Name = view.Name, IsAccepted = true });
    }

    [HttpPut("{tagId}")]
    public async Task<IActionResult> UpdateTag(Guid tagId, UpdateTagCommand command)
    {
        if (tagId != command.TagId)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }
}
