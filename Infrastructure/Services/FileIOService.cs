using Application.Common.Interfaces;

namespace Infrastructure.Services;

public class FileIOService : IFileIOService
{
	public bool Exists(string path)
	{
		return File.Exists(path);
	}
	public async Task<string> ReadAllTextAsync(string path)
	{
		return await File.ReadAllTextAsync(path);
	}
	public async Task WriteAllText(string path, string text)
	{
		await File.WriteAllTextAsync(path, text);
	}
}
