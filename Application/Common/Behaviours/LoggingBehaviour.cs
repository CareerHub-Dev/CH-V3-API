using Application.Common.Interfaces;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace Application.Common.Behaviours;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger _logger;
    private readonly ICurrentAccountService _currentAccountService;

    public LoggingBehaviour(ILogger<TRequest> logger, ICurrentAccountService currentAccountService)
    {
        _logger = logger;
        _currentAccountService = currentAccountService;
    }

    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = _currentAccountService.AccountId ?? string.Empty;

        _logger.LogInformation("CH-V3-API Request: {Name} {@UserId} {@Request}", requestName, userId, request);

        return Task.CompletedTask;
    }
}
