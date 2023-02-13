using API.Authorize;
using Microsoft.AspNetCore.SignalR;

namespace API.Providers;

public class CustomUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        return (connection.GetHttpContext()?.Items["Account"] as AccountInfo)?.Id.ToString();
    }
}
