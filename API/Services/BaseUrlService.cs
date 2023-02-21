using Application.Common.Interfaces;

namespace API.Services;

public class BaseUrlService : IBaseUrlService
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public BaseUrlService(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    public string? GetBaseUrl()
    {
        return httpContextAccessor.HttpContext?.Request.BaseUrl();
    }
}

public static class HttpRequestExtensions
{
    public static string? BaseUrl(this HttpRequest req)
    {
        if (req == null) return null;
        var uriBuilder = new UriBuilder(req.Scheme, req.Host.Host, req.Host.Port ?? -1);
        if (uriBuilder.Uri.IsDefaultPort)
        {
            uriBuilder.Port = -1;
        }

        return uriBuilder.Uri.AbsoluteUri;
    }
}