using Application.Emails.Commands.SendInviteAdminEmail;
using Domain.Entities;
using MediatR;

namespace Application.Admins.Events;

public class AdminInvitedEvent
    : INotification
{
    public AdminInvitedEvent(Admin admin)
    {
        Admin = admin;
    }

    public Admin Admin { get; }
}

public class AdminInvitedEventHandler
    : INotificationHandler<AdminInvitedEvent>
{
    private readonly ISender _sender;

    public AdminInvitedEventHandler(
        ISender sender)
    {
        _sender = sender;
    }

    public async Task Handle(
        AdminInvitedEvent notification,
        CancellationToken cancellationToken)
    {
        await _sender.Send(new SendInviteAdminEmailCommand(notification.Admin.Id));
    }
}
