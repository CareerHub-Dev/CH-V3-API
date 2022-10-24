using Microsoft.AspNetCore.Http;

namespace Application.Common.Interfaces;

public interface IFileService
{
	bool Exists(string path);
	Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken = default);
	Task WriteAllTextAsync(string path, string text, CancellationToken cancellationToken = default);
	Task<string> MoveFileAsync(IFormFile file, string directoryPath, CancellationToken cancellationToken = default);
    void RemoveFile(string path);
}
