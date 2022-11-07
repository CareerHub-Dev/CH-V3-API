using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.IO.Abstractions;

namespace Application.Services;

public class ImagesService : IImagesService
{
    private readonly IPathService _pathService;
    private readonly IFileSystem _fileSystem;
    public ImagesService(IPathService pathService, IFileSystem fileSystem)
    {
        _pathService = pathService;
        _fileSystem = fileSystem;
    }

    public List<string> GetImageFileNames()
    {
        var imagesRoute = _pathService.GetImagesRoute;
        var imagesDirectoryPath = _fileSystem.Path.Combine(_pathService.GetWebRootPath, imagesRoute);

        var directory = _fileSystem.DirectoryInfo.FromDirectoryName(imagesDirectoryPath);

        var files = directory.GetFiles();

        return files.Select(x => x.Name).ToList();
    }

    public async Task<string> SaveImageAsync(IFormFile image, CancellationToken cancellationToken = default)
    {
        var imagesRoute = _pathService.GetImagesRoute;
        var imagesDirectoryPath = _fileSystem.Path.Combine(_pathService.GetWebRootPath, imagesRoute);

        if (!_fileSystem.Directory.Exists(imagesDirectoryPath))
        {
            _fileSystem.Directory.CreateDirectory(imagesDirectoryPath);
        }

        var uniqueFileName = _fileSystem.Path.GetRandomFileName() + _fileSystem.Path.GetExtension(image.FileName);

        var imagePath = _fileSystem.Path.Combine(imagesDirectoryPath, uniqueFileName);

        if (_fileSystem.File.Exists(imagePath))
        {
            return await SaveImageAsync(image, cancellationToken);
        }

        using var stream = _fileSystem.FileStream.Create(imagePath, FileMode.Create);
        await image.CopyToAsync(stream, cancellationToken);

        return Path.Combine(imagesRoute, uniqueFileName);
    }

    public void DeleteImageIfExists(string imageFileName)
    {
        var imagesRoute = _pathService.GetImageRoute(imageFileName);
        var imagePath = _fileSystem.Path.Combine(_pathService.GetWebRootPath, imagesRoute);

        if (_fileSystem.File.Exists(imagePath))
        {
            _fileSystem.File.Delete(imagePath);
        }
    }

    public void DeleteImagesIfExists(IEnumerable<string> imageFileNames)
    {
        var imagesRoute = _pathService.GetImagesRoute;
        var imagesDirectoryPath = _fileSystem.Path.Combine(_pathService.GetWebRootPath, imagesRoute);

        foreach (var imageFileName in imageFileNames)
        {
            var imagePath = _fileSystem.Path.Combine(imagesDirectoryPath, imageFileName);

            if (_fileSystem.File.Exists(imagePath))
            {
                _fileSystem.File.Delete(imagePath);
            }
        }
    }
}
