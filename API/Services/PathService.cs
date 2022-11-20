using Application.Common.Interfaces;

namespace API.Services;

public class PathService 
    : IPathService
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public PathService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public string GetContentRootPath => _webHostEnvironment.ContentRootPath;
    public string GetWebRootPath => _webHostEnvironment.WebRootPath;

    public string GetEmailTemplatesRoute => "EmailTemplates";
    public string GetEmailTemplateRoute(string templateName) => Path.Combine("EmailTemplates", templateName);

    public string GetImagesRoute => "Images";
    public string GetImageRoute(string imageName) => Path.Combine("Images", imageName);
}
