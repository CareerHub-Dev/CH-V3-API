using Application.Emails.Commands;
using Domain.Events.Company;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Companies.Events;

public class CompanyInvitedEventHandler : INotificationHandler<CompanyInvitedEvent>
{
    private readonly ILogger<CompanyInvitedEventHandler> _logger;
    private readonly IMediator _mediator;

    public CompanyInvitedEventHandler(ILogger<CompanyInvitedEventHandler> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Handle(CompanyInvitedEvent notification, CancellationToken cancellationToken)
    {
        await _mediator.Send(new SendInviteCompanyEmailCommand(notification.Company.Id), cancellationToken);

        _logger.LogInformation("CH-V3-API Domain Event: {DomainEvent}", notification.GetType().Name);
    }
}
