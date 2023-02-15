using Domain.Enums;

namespace Domain.Entities;

public class Notification
{
    public Guid Id { get; set; }

    public Guid ReferenceId { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Image { get; set; }
    public bool IsViewed { get; set; }
    public DateTime Created { get; set; }
    public NotificationType Type { get; set; }

    public Guid StudentId { get; set; }
    public Student? Student { get; set; }
}
