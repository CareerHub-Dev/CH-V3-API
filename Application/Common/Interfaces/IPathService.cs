namespace Application.Common.Interfaces;

public interface IPathService
{
    string GetPhysicalPath();
    string GetEmailTemplatePath(string templateName);
    string GetImagesPath();
    string GetImagePath(string imageName);
}
