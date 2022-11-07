using Microsoft.AspNetCore.Http;

namespace Application.Common.Interfaces;

public interface IImagesService
{
    Task<string> SaveImageAsync(IFormFile image, CancellationToken cancellationToken = default);
    void DeleteImageIfExists(string imageFileName);
    void DeleteImagesIfExists(IEnumerable<string> imageFileNames);
    List<string> GetImageFileNames();
}
