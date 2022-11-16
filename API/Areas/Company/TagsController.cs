using API.Authorize;
using API.DTO.Requests.Tags;
using Application.Common.Enums;
using Application.Tags.Commands.CreateTag;
using Microsoft.AspNetCore.Mvc;

namespace API.Areas.Company;

[Authorize(Role.Company)]
[Route("api/Company/[controller]")]
public class TagsController : ApiControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    public async Task<IActionResult> CreateTag(CreateTagRequest request)
    {
        var result = await Sender.Send(new CreateTagCommand { Name = request.Name, IsAccepted = false });

        return Ok(result);
    }
}
