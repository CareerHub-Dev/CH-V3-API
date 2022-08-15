using Domain.Common;

namespace Domain.Events.Company;

public class CompanyInvitedEvent : BaseEvent
{
    public CompanyInvitedEvent(Entities.Company company)
    {
        Company = company;
    }

    public Entities.Company Company { get; }
}
