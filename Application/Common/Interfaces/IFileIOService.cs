namespace Application.Common.Interfaces;

public interface IFileIOService
{
	bool Exists(string path);
	Task<string> ReadAllTextAsync(string path);
	Task WriteAllText(string path, string text);
}
