using Application.Common.Interfaces;

namespace Infrastructure.Services;

public class FileIOService : IFileIOService
    {
	public bool Exists(string path)
	{
		return File.Exists(path);
	}
	public string ReadAllText(string path)
	{
		return File.ReadAllText(path);
	}
	public void WriteAllText(string path, string text)
	{
		File.WriteAllText(path, text);
	}
}
