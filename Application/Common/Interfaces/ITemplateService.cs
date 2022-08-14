namespace Application.Common.Interfaces;

public interface ITemplateService
{
    Task<string> GetTemplateAsync(string templateName);
}
