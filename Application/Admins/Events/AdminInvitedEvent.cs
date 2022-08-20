using Application.Emails.Commands;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

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
    private readonly ILogger<AdminInvitedEventHandler> _logger;
    private readonly IMediator _mediator;

    public AdminInvitedEventHandler(ILogger<AdminInvitedEventHandler> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Handle(AdminInvitedEvent notification, CancellationToken cancellationToken)
    {
        await _mediator.Send(new SendInviteAdminEmailCommand(notification.Admin.Id), cancellationToken);

        _logger.LogInformation("CH-V3-API Application Event: {ApplicationEvent}", notification.GetType().Name);
    }
}
