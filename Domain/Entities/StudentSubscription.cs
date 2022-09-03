namespace Domain.Entities;

public class StudentSubscription
{
    public Guid SubscriptionOwnerId { get; set; }
    public Student? SubscriptionOwner { get; set; }

    public Guid SubscriptionTargetId { get; set; }
    public Student? SubscriptionTarget { get; set; }
}
