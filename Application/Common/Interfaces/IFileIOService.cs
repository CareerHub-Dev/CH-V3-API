namespace Application.Common.Interfaces;

public interface IFileIOService
{
	bool Exists(string path);
	string ReadAllText(string path);
	void WriteAllText(string path, string text);
}
