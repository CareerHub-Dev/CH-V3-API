namespace Application.Common.Interfaces;

public interface IMailKitService
{
    Task SendEmailAsync(string to, string subject, string html, string? from = null, CancellationToken cancellationToken = default);
}
