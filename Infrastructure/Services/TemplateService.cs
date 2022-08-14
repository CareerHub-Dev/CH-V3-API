using Application.Common.Interfaces;
using System.Reflection;

namespace Infrastructure.Services;

public class TemplateService : ITemplateService
{
    private readonly string _templatesPath;
    private readonly IFileIOService _fileIOService;

    public TemplateService(IFileIOService fileIOService)
    {
        var baseRoot = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;

        _templatesPath = Path.Combine(baseRoot, "Templates");
        _fileIOService = fileIOService;
    }

    public async Task<string> GetTemplateAsync(string templateName)
    {
        var path = Path.Combine(_templatesPath, templateName);
        if (!_fileIOService.Exists(path))
        {
            throw new FileNotFoundException(path);
        }

        return await _fileIOService.ReadAllTextAsync(path);
    }
}
