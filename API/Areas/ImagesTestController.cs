using Application.Common.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Areas;

[Route("api/[controller]")]
public class ImagesTestController : ApiControllerBase
{
    private readonly IPathService _pathService;

    public ImagesTestController(IPathService pathService)
    {
        _pathService = pathService;
    }

    [HttpDelete("delete/{fileName}")]
    public IActionResult Delete(string fileName)
    {
        FileService fileService = new FileService();

        fileService.RemoveFile(_pathService.GetImagePath(fileName));

        return Ok();
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(IFormFile file)
    {
        FileService fileService = new FileService();

        return Ok(await fileService.MoveFileAsync(file, _pathService.GetImagesPath()));
    }
}
