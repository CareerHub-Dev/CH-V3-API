using Application.Common.Models.Image;

namespace WebUI.Common.Extentions;

public static class FormFileExtensions
{
    public static async Task<byte[]> ToByteArrayAsync(this IFormFile formFile)
    {
        await using var memoryStream = new MemoryStream();
        await formFile.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }

    public static async Task<CreateImage> ToCreateImageAsync(this IFormFile formFile)
    {
        return new CreateImage
        {
            ContentType = formFile.ContentType,
            Content = await formFile.ToByteArrayAsync()
        };
    }
}
