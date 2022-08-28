using Application.Emails.Commands;
using Domain.Entities;
using MediatR;

namespace Application.Companies.Events;

public class CompanyInvitedEvent : INotification
{
    public CompanyInvitedEvent(Company company)
    {
        Company = company;
    }

    public Company Company { get; }
}

public class CompanyInvitedEventHandler : INotificationHandler<CompanyInvitedEvent>
{
    private readonly IMediator _mediator;

    public CompanyInvitedEventHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(CompanyInvitedEvent notification, CancellationToken cancellationToken)
    {
        await _mediator.Send(new SendInviteCompanyEmailCommand(notification.Company.Id), cancellationToken);
    }
}
