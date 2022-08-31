using Application.Common.Interfaces;

namespace Application.Services;

public class EmailTemplatesService : IEmailTemplatesService
{
    private readonly IPathService _pathService;
    private readonly IFileIOService _fileIOService;
    public EmailTemplatesService(IPathService pathService, IFileIOService fileIOService)
    {
        _pathService = pathService;
        _fileIOService = fileIOService;
    }

    public async Task<string> ReadTemplateAsync(string templateName, CancellationToken cancellationToken = default)
    {
        var templatePath = _pathService.GetEmailTemplatePath(templateName);

        if (!_fileIOService.Exists(templatePath))
        {
            throw new FileNotFoundException($"Could not find file '{templatePath}'.");
        }

        var emailTemplate = await _fileIOService.ReadAllTextAsync(templatePath, cancellationToken);

        if (string.IsNullOrEmpty(emailTemplate))
        {
            throw new ArgumentException($"Template is empty by path: '{templateName}'.");
        }

        return emailTemplate;
    }
}
