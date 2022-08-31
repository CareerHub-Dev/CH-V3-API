namespace Application.Common.Interfaces;

public interface IMailKitService
{
    Task SendAsync(string to, string subject, string html, string? from = null, CancellationToken cancellationToken = default);
}
