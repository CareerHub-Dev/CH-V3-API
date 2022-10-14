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

    public async Task MoveFileAsync(IFormFile file, string directoryPath, string uniqueFileName, CancellationToken cancellationToken = default)
    {
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        var imagePath = Path.Combine(directoryPath, uniqueFileName);

        using var stream = new FileStream(imagePath, FileMode.Create);
        await file.CopyToAsync(stream, cancellationToken);
    }

    public async Task ReplaceFileIfExistAsync(
        IFormFile file, 
        string directoryPath, 
        string uniqueNewFileName, 
        string previousFileName, 
        CancellationToken cancellationToken = default)
    {
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        var previousImagePath = Path.Combine(directoryPath, previousFileName);

        if (Exists(previousImagePath))
        {
            RemoveFile(previousImagePath);
        }

        await MoveFileAsync(file, directoryPath, uniqueNewFileName, cancellationToken);
    }

    public void RemoveFile(string path) => File.Delete(path);
}
