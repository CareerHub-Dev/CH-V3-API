namespace Application.Common.Interfaces;

public interface IFileIOService
{
	bool Exists(string path);
	Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken = default);
	Task WriteAllTextAsync(string path, string text, CancellationToken cancellationToken = default);
}
