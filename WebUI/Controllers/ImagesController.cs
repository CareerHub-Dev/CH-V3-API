using Application.Images.Queries.GetImage;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;

namespace WebUI.Controllers
{
    public class ImagesController : ApiControllerBase
    {
        /// <summary>
        /// Auth
        /// </summary>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetImage(Guid id)
        {
            var image = await Mediator.Send(new GetImageQuery(id));

            return File(image.Content, $"image/{image.Extention[1..]}");
        }
    }
}
