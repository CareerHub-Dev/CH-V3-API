using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Entensions;

public static class FormFileExtensions
{
    public static async Task<byte[]> ToByteArrayAsync(this IFormFile formFile)
    {
        await using var memoryStream = new MemoryStream();
        await formFile.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }

    public static async Task<Image> ToImageWithGeneratedIdAsync(this IFormFile formFile)
    {
        return new Image
        {
            Id = Guid.NewGuid(),
            ContentType = formFile.ContentType,
            Content = await formFile.ToByteArrayAsync()
        };
    }
}
