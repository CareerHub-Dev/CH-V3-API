using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public class FileService : IFileService
{
	public bool Exists(string path) => File.Exists(path);

    public async Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken = default)
	{
		return await File.ReadAllTextAsync(path, cancellationToken);
	}

	public async Task WriteAllTextAsync(string path, string text, CancellationToken cancellationToken = default)
	{
		await File.WriteAllTextAsync(path, text, cancellationToken);
	}

    public async Task<string> MoveFileAsync(IFormFile file, string directoryPath, CancellationToken cancellationToken = default)
    {
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        var uniqueFileName = Path.GetRandomFileName() + Path.GetExtension(file.FileName);

        var imagePath = Path.Combine(directoryPath, uniqueFileName);

        using var stream = new FileStream(imagePath, FileMode.Create);
        await file.CopyToAsync(stream, cancellationToken);

        return imagePath;
    }

    public void RemoveFile(string path) => File.Delete(path);
}
