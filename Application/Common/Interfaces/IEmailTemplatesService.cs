namespace Application.Common.Interfaces;

public interface IEmailTemplatesService
{
    Task<string> ReadTemplateAsync(string templateName, CancellationToken cancellationToken = default);
}
