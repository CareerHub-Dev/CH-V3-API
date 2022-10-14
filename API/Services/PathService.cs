using Application.Common.Interfaces;

namespace API.Services;

public class PathService : IPathService
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public PathService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public string GetPhysicalPath() => _webHostEnvironment.ContentRootPath;
    public string GetEmailTemplatePath(string templateName) => Path.Combine(_webHostEnvironment.WebRootPath, "EmailTemplates", templateName);
    public string GetImagesPath() => Path.Combine(_webHostEnvironment.WebRootPath, "Images");
    public string GetImagePath(string imageName) => Path.Combine(_webHostEnvironment.WebRootPath, "Images", imageName);
}
