using Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using OneSignalApi.Api;
using OneSignalApi.Client;
using OneSignalApi.Model;

namespace Application.Services.Notifications;

public class NotificationService : INotificationService
{
    private readonly IConfiguration _configuration;

	public NotificationService(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	public async Task SendNotificationAsync(List<Guid> userIds, string webUrl = "", string enMessage = "", string ukMessage = "", string largeIcon = "")
	{
        var appConfig = new Configuration();
        appConfig.BasePath = "https://onesignal.com/api/v1";
        appConfig.AccessToken = _configuration["OneSignal:ApiKey"];
        var appInstance = new DefaultApi(appConfig);

        var notification = new Notification(appId: _configuration["OneSignal:AppID"])
        {
            Contents = new StringMap(en: enMessage, uk: ukMessage),
            IncludedSegments = new List<string> { "included_player_ids" },
            IncludePlayerIds = userIds.Select(x => x.ToString()).ToList(),
            WebUrl = webUrl,
            LargeIcon = largeIcon
        };

        await appInstance.CreateNotificationAsync(notification);
    }
}
