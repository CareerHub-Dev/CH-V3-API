using Application.Tags.Commands.CreateTag;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;
using WebUI.ViewModels.Tags;

namespace WebUI.Areas.Company;

[Authorize("Company")]
[Route("api/Company/[controller]")]
public class TagsController : ApiControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    public async Task<IActionResult> CreateTag(CreateTagView view)
    {
        var result = await Mediator.Send(new CreateTagCommand { Name = view.Name, IsAccepted = false });

        return Ok(result);
    }
}
