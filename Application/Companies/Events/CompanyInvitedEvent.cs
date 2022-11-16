using Application.Emails.Commands.SendInviteCompany;
using Domain.Entities;
using MediatR;

namespace Application.Companies.Events;

public class CompanyInvitedEvent
    : INotification
{
    public CompanyInvitedEvent(Company company)
    {
        Company = company;
    }

    public Company Company { get; }
}

public class CompanyInvitedEventHandler
    : INotificationHandler<CompanyInvitedEvent>
{
    private readonly ISender _sender;

    public CompanyInvitedEventHandler(
        ISender sender)
    {
        _sender = sender;
    }

    public async Task Handle(
        CompanyInvitedEvent notification, 
        CancellationToken cancellationToken)
    {
        await _sender.Send(new SendInviteCompanyEmailCommand(notification.Company.Id));
    }
}
