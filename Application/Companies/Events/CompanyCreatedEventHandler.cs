using Application.Emails.Commands;
using Domain.Events.Company;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Companies.Events;

public class CompanyCreatedEventHandler : INotificationHandler<CompanyCreatedEvent>
{
    private readonly ILogger<CompanyCreatedEventHandler> _logger;
    private readonly IMediator _mediator;

    public CompanyCreatedEventHandler(ILogger<CompanyCreatedEventHandler> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Handle(CompanyCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("CH-V3-API Domain Event: {DomainEvent}", notification.GetType().Name);

        await _mediator.Send(new SendInviteCompanyEmailCommand(notification.Company.Id), cancellationToken);
    }
}
