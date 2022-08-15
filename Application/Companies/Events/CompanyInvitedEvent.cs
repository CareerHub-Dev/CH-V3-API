using Application.Common.Entensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Domain.Entities;
using Microsoft.Extensions.Logging;

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
    private readonly ILogger<CompanyInvitedEventHandler> _logger;
    private readonly IEmailService _emailService;
    private readonly ITemplateService _templateService;

    public CompanyInvitedEventHandler(ILogger<CompanyInvitedEventHandler> logger, IEmailService emailService, ITemplateService templateService)
    {
        _logger = logger;
        _emailService = emailService;
        _templateService = templateService;
    }

    public async Task Handle(CompanyInvitedEvent notification, CancellationToken cancellationToken)
    {
        var template = await _templateService.GetTemplateAsync(TemplateConstants.CompanyInvitationEmail);

        template = template.MultipleReplace(new Dictionary<string, string> { { "{verificationToken}", notification.Company.VerificationToken ?? "" } });

        await _emailService.SendEmailAsync(notification.Company.NormalizedEmail, "Invitation Email", template);

        _logger.LogInformation("CH-V3-API Application Event: {ApplicationEvent}", notification.GetType().Name);
    }
}