using Domain.Events.Company;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Companies.Events;

public class CompanyCreatedEventHandler : INotificationHandler<CompanyCreatedEvent>
{
    private readonly ILogger<CompanyCreatedEventHandler> _logger;

    public CompanyCreatedEventHandler(ILogger<CompanyCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(CompanyCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("CH-V3-API Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
