using Application.Common.Interfaces;

namespace API.Services;

public class СurrentRemoteIpAddressService 
    : IСurrentRemoteIpAddressService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public СurrentRemoteIpAddressService(
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string IpAddress
    {
        get
        {
            var context = _httpContextAccessor.HttpContext;

            if(context == null)
            {
                return string.Empty;
            }

            if (context.Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                return context.Request.Headers["X-Forwarded-For"]!;
            }

            var remoteIpAddress = context.Connection.RemoteIpAddress;

            if (remoteIpAddress != null)
            {
                return remoteIpAddress.MapToIPv4().ToString();
            }

            return string.Empty;
        }
    }
}
