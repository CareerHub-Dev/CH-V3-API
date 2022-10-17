using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Application.Services;

public class ImagesService : IImagesService
{
    private readonly IPathService _pathService;
    private readonly IFileService _fileService;
    public ImagesService(IPathService pathService, IFileService fileIOService)
    {
        _pathService = pathService;
        _fileService = fileIOService;
    }

    public async Task<string> SaveImageAsync(IFormFile image, CancellationToken cancellationToken = default)
    {
        var imagesRoute = _pathService.GetImagesRoute;
        var imagesDirectoryPath = Path.Combine(_pathService.GetWebRootPath, imagesRoute);

        var imageName = await _fileService.MoveFileAsync(image, imagesDirectoryPath, cancellationToken);

        return Path.Combine(imagesRoute, imageName);
    }

    public void DeleteImageIfExists(string imageFileName)
    {
        var imagesRoute = _pathService.GetImageRoute(imageFileName);
        var imagePath = Path.Combine(_pathService.GetWebRootPath, imagesRoute);

        if (_fileService.Exists(imagePath))
        {
            _fileService.RemoveFile(imagePath);
        }
    }

    public async Task<string> ReplaceImageAsync(string oldImageFileName, IFormFile newImage, CancellationToken cancellationToken = default)
    {
        DeleteImageIfExists(oldImageFileName);
        return await SaveImageAsync(newImage, cancellationToken);
    }
}
