using Application.Emails.Commands;
using Domain.Entities;
using MediatR;

namespace Application.Admins.Events;

public class AdminInvitedEvent : INotification
{
    public AdminInvitedEvent(Admin admin)
    {
        Admin = admin;
    }

    public Admin Admin { get; }
}

public class AdminInvitedEventHandler : INotificationHandler<AdminInvitedEvent>
{
    private readonly IMediator _mediator;

    public AdminInvitedEventHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(AdminInvitedEvent notification, CancellationToken cancellationToken)
    {
        await _mediator.Send(new SendInviteAdminEmailCommand(notification.Admin.Id), cancellationToken);
    }
}
