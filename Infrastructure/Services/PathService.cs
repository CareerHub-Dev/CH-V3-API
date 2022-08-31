using Application.Common.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace Infrastructure.Services;

public class PathService : IPathService
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public PathService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public string GetPhysicalPath(string path)
    {
        return Path.Combine(_webHostEnvironment.ContentRootPath, path);
    }

    public string GetEmailTemplatePath(string templateName)
    {
        var path = Path.Combine("wwwroot", "EmailTemplates", templateName);
        return path;
    }
}
