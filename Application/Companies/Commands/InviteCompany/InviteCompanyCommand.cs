using Application.Common.Interfaces;
using Application.Companies.Events;
using Domain.Entities;
using Domain.Events.Company;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Application.Companies.Commands.InviteCompany;

public record InviteCompanyCommand : IRequest<Guid>
{
    public string Email { get; init; } = string.Empty;
}

public class InviteCompanyCommandHandler : IRequestHandler<InviteCompanyCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;

    public InviteCompanyCommandHandler(IApplicationDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<Guid> Handle(InviteCompanyCommand request, CancellationToken cancellationToken)
    {
        var entity = new Company
        {
            Email = request.Email,
            VerificationToken = await GenerateVerificationTokenAsync()
        };

        entity.AddDomainEvent(new CompanyCreatedEvent(entity));

        await _context.Companies.AddAsync(entity, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        await _mediator.Publish(new CompanyInvitedEvent(entity), cancellationToken);

        return entity.Id;
    }

    private async Task<string> GenerateVerificationTokenAsync()
    {
        // token is a cryptographically strong random sequence of values
        var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));

        // ensure token is unique by checking against db
        var tokenIsUnique = !await _context.Accounts.AnyAsync(x => x.VerificationToken == token);
        if (!tokenIsUnique)
            return await GenerateVerificationTokenAsync();

        return token;
    }
}