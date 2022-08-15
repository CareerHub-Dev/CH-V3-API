using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Events.Company;
using MediatR;

namespace Application.Companies.Commands.InviteCompany;

public record InviteCompanyCommand : IRequest<Guid>
{
    public string Email { get; init; } = string.Empty;
}

public class InviteCompanyCommandHandler : IRequestHandler<InviteCompanyCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public InviteCompanyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(InviteCompanyCommand request, CancellationToken cancellationToken)
    {
        var entity = new Company
        {
            Email = request.Email
        };

        entity.AddDomainEvent(new CompanyInvitedEvent(entity));

        await _context.Companies.AddAsync(entity, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}