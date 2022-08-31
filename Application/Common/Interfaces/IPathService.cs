namespace Application.Common.Interfaces;

public interface IPathService
{
    string GetEmailTemplatePath(string templateName);
    string GetPhysicalPath(string path);
}
