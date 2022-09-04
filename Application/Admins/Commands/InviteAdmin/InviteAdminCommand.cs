﻿using Application.Admins.Events;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Admins.Commands.InviteAdmin;

public record InviteAdminCommand : IRequest<Guid>
{
    public string Email { get; init; } = string.Empty;
    public bool IsSuperAdmin { get; init; }
}

public class InviteAdminCommandHandler : IRequestHandler<InviteAdminCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;

    public InviteAdminCommandHandler(IApplicationDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<Guid> Handle(InviteAdminCommand request, CancellationToken cancellationToken)
    {
        var admin = new Admin
        {
            Email = request.Email,
            IsSuperAdmin = request.IsSuperAdmin,
        };

        await _context.Admins.AddAsync(admin);

        await _context.SaveChangesAsync();

        await _mediator.Publish(new AdminInvitedEvent(admin));

        return admin.Id;
    }
}
