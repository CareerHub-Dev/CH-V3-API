using Application.Common.Interfaces;

namespace Infrastructure.Services;

public class FileIOService : IFileIOService
{
	public bool Exists(string path)
	{
		return File.Exists(path);
	}
	public async Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken = default)
	{
		return await File.ReadAllTextAsync(path, cancellationToken);
	}
	public async Task WriteAllTextAsync(string path, string text, CancellationToken cancellationToken = default)
	{
		await File.WriteAllTextAsync(path, text, cancellationToken);
	}
}
