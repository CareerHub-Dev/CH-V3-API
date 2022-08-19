using Application.Emails.Commands;
using Domain.Events.Admin;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Companies.Events;

public class AdminCreatedEventHandler : INotificationHandler<AdminCreatedEvent>
{
    private readonly ILogger<CompanyCreatedEventHandler> _logger;
    private readonly IMediator _mediator;

    public AdminCreatedEventHandler(ILogger<CompanyCreatedEventHandler> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Handle(AdminCreatedEvent notification, CancellationToken cancellationToken)
    {
        await _mediator.Send(new SendInviteAdminEmailCommand(notification.Admin.Id), cancellationToken);

        _logger.LogInformation("CH-V3-API Domain Event: {DomainEvent}", notification.GetType().Name);
    }
}
