using Application.Common.Interfaces;

namespace WebUI.Services;

public class СurrentRemoteIpAddressService : IСurrentRemoteIpAddressService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public СurrentRemoteIpAddressService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string IpAddress
    {
        get
        {
            if (_httpContextAccessor.HttpContext!.Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                return _httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"];
            }
            else
            {
                return _httpContextAccessor.HttpContext.Connection.RemoteIpAddress!.MapToIPv4().ToString();
            }
        }
    }
}
