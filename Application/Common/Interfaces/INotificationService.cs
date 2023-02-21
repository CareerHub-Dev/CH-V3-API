namespace Application.Common.Interfaces;

public interface INotificationService
{
    Task SendNotificationAsync(List<Guid> userIds, string appUrl = "", string enMessage = "", string ukMessage = "", string largeIcon = "", object? data = null);
}
