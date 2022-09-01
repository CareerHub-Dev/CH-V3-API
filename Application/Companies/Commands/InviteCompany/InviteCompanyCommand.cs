﻿using Application.Common.Interfaces;
using Application.Companies.Events;
using Domain.Entities;
using MediatR;

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
            Email = request.Email
        };

        await _context.Companies.AddAsync(entity);

        await _context.SaveChangesAsync();

        await _mediator.Publish(new CompanyInvitedEvent(entity));

        return entity.Id;
    }
}