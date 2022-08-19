using Domain.Common;

namespace Domain.Events.Company;

public class CompanyCreatedEvent : BaseEvent
{
    public CompanyCreatedEvent(Entities.Company company)
    {
        Company = company;
    }

    public Entities.Company Company { get; }
}
