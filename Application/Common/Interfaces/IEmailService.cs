namespace Application.Common.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string html, string? from = null);
}
