using Application.Common.Interfaces;
using System.IO.Abstractions;

namespace Application.Services;

public class EmailTemplatesService : IEmailTemplatesService
{
    private readonly IPathService _pathService;
    private readonly IFileSystem _fileSystem;
    public EmailTemplatesService(IPathService pathService, IFileSystem fileSystem)
    {
        _pathService = pathService;
        _fileSystem = fileSystem;
    }

    public async Task<string> ReadTemplateAsync(string templateName, CancellationToken cancellationToken = default)
    {
        var templatePath = Path.Combine(_pathService.GetWebRootPath, _pathService.GetEmailTemplateRoute(templateName));

        if (!_fileSystem.File.Exists(templatePath))
        {
            throw new FileNotFoundException($"Could not find file '{templatePath}'.");
        }

        var emailTemplate = await _fileSystem.File.ReadAllTextAsync(templatePath, cancellationToken);

        if (string.IsNullOrEmpty(emailTemplate))
        {
            throw new ArgumentException($"Template is empty by path: '{templateName}'.");
        }

        return emailTemplate;
    }
}
