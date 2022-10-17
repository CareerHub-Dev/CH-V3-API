using Microsoft.AspNetCore.Http;

namespace Application.Common.Interfaces;

public interface IImagesService
{
    Task<string> SaveImageAsync(IFormFile image, CancellationToken cancellationToken = default);
    void DeleteImageIfExists(string imageFileName);
    Task<string> ReplaceImageAsync(string oldImageFileName, IFormFile newImage, CancellationToken cancellationToken = default);
}
