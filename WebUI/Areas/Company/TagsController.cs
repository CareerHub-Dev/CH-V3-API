using Application.Tags.Commands.CreateTag;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;
using WebUI.Common.Models.Tag;

namespace WebUI.Areas.Company;

[Authorize]
[Route("api/Company/[controller]")]
public class TagsController : ApiControllerBase
{
    [HttpPost]
    public async Task<Guid> CreateTag(CreateTagView view)
    {
        return await Mediator.Send(new CreateTagCommand { Name = view.Name, IsAccepted = false });
    }
}
