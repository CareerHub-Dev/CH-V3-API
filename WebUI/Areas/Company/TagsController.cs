using Application.Tags.Commands.CreateTag;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;
using WebUI.DTO.Requests.Tags;

namespace WebUI.Areas.Company;

[Authorize("Company")]
[Route("api/Company/[controller]")]
public class TagsController : ApiControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    public async Task<IActionResult> CreateTag(CreateTagRequest view)
    {
        var result = await Mediator.Send(new CreateTagCommand { Name = view.Name, IsAccepted = false });

        return Ok(result);
    }
}
