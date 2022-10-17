namespace Application.Common.Interfaces;

public interface IPathService
{
    public string GetContentRootPath { get; }
    public string GetWebRootPath { get; }

    public string GetEmailTemplatesRoute { get; }
    string GetEmailTemplateRoute(string templateName);
    public string GetImagesRoute { get; }
    string GetImageRoute(string imageName);
}
