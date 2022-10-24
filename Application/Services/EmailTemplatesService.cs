using Application.Common.Interfaces;

namespace Application.Services;

public class EmailTemplatesService : IEmailTemplatesService
{
    private readonly IPathService _pathService;
    private readonly IFileService _fileService;
    public EmailTemplatesService(IPathService pathService, IFileService fileIOService)
    {
        _pathService = pathService;
        _fileService = fileIOService;
    }

    public async Task<string> ReadTemplateAsync(string templateName, CancellationToken cancellationToken = default)
    {
        var templatePath = Path.Combine(_pathService.GetWebRootPath, _pathService.GetEmailTemplateRoute(templateName));

        if (!_fileService.Exists(templatePath))
        {
            throw new FileNotFoundException($"Could not find file '{templatePath}'.");
        }

        var emailTemplate = await _fileService.ReadAllTextAsync(templatePath, cancellationToken);

        if (string.IsNullOrEmpty(emailTemplate))
        {
            throw new ArgumentException($"Template is empty by path: '{templateName}'.");
        }

        return emailTemplate;
    }
}
