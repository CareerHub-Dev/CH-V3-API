using Domain.Enums;

namespace Application.Common.DTO.Notifications;

public class NotificationDTO
{
    public Guid Id { get; set; }

    public Guid ReferenceId { get; set; }
    public string UkMessage { get; set; } = string.Empty;
    public string EnMessage { get; set; } = string.Empty;
    public string? Image { get; set; }
    public bool IsViewed { get; set; }
    public DateTime Created { get; set; }
    public NotificationType Type { get; set; }
}
