using Domain.Common;

namespace Domain.Events.Admin;

public class AdminCreatedEvent : BaseEvent
{
    public AdminCreatedEvent(Entities.Admin company)
    {
        Admin = company;
    }

    public Entities.Admin Admin { get; }
}
