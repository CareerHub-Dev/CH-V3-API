using Application.Common.Models.Tag;
using Application.Tags.Queries;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;

namespace WebUI.Areas.Auth;

[Authorize]
[Route("api/Auth/[controller]")]
public class TagsController : ApiControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<TagBriefDTO>> GetJobPositions([FromQuery] GetTagBriefsWithSearchQuery query)
    {
        return await Mediator.Send(query);
    }
}
