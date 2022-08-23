using Application.Images.Queries;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;

namespace WebUI.Controllers;

public class ImagesController : ApiControllerBase
{
    /// <summary>
    /// Auth
    /// </summary>
    [HttpGet("{imageId}")]
    [Authorize]
    public async Task<IActionResult> GetImage(Guid imageId)
    {
        var image = await Mediator.Send(new GetImageQuery(imageId));

        return File(image.Content, $"image/{image.Extention[1..]}");
    }
}
