using Microsoft.AspNetCore.Http;

namespace Application.Common.Interfaces;

public interface IFileService
{
	bool Exists(string path);
	Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken = default);
	Task WriteAllTextAsync(string path, string text, CancellationToken cancellationToken = default);
	Task MoveFileAsync(IFormFile file, string directoryPath, string uniqueFileName, CancellationToken cancellationToken = default);
	void RemoveFile(string path);
	Task ReplaceFileIfExistAsync(IFormFile file, string directoryPath, string uniqueNewFileName, string previousFileName, CancellationToken cancellationToken = default);
}
