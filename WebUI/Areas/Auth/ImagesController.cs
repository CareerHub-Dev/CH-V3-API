using Application.Images.Queries;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;

namespace WebUI.Areas.Auth;

[Authorize]
[Route("api/Auth/[controller]")]
public class ImagesController : ApiControllerBase
{
    [HttpGet("{imageId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileContentResult))]
    public async Task<IActionResult> GetImage(Guid imageId)
    {
        var image = await Mediator.Send(new GetImageQuery(imageId));

        return File(image.Content, image.ContentType);
    }
}
