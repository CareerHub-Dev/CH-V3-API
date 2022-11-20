using Application.Common.Interfaces;
using Application.Companies.Events;
using Domain.Entities;
using MediatR;

namespace Application.Companies.Commands.InviteCompany;

public record InviteCompanyCommand
    : IRequest<Guid>
{
    public string Email { get; init; } = string.Empty;
}

public class InviteCompanyCommandHandler
    : IRequestHandler<InviteCompanyCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IPublisher _publisher;

    public InviteCompanyCommandHandler(
        IApplicationDbContext context,
        IPublisher publisher)
    {
        _context = context;
        _publisher = publisher;
    }

    public async Task<Guid> Handle(
        InviteCompanyCommand request,
        CancellationToken cancellationToken)
    {
        var company = new Company
        {
            Email = request.Email
        };

        await _context.Companies.AddAsync(company);

        await _context.SaveChangesAsync();

        await _publisher.Publish(new CompanyInvitedEvent(company));

        return company.Id;
    }
}