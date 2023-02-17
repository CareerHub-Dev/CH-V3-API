namespace Application.Common.Interfaces;

public interface INotificationService
{
    Task SendNotificationAsync(List<Guid> userIds, string webUrl = "", string enMessage = "", string ukMessage = "", string largeIcon = "");
}
