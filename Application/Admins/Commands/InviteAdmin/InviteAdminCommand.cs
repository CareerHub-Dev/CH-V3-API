using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Events.Admin;
using MediatR;

namespace Application.Admins.Commands.InviteAdmin;

public record InviteAdminCommand : IRequest<Guid>
{
    public string Email { get; init; } = string.Empty;
}

public class InviteAdminCommandHandler : IRequestHandler<InviteAdminCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public InviteAdminCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(InviteAdminCommand request, CancellationToken cancellationToken)
    {
        var entity = new Admin
        {
            Email = request.Email
        };

        entity.AddDomainEvent(new AdminCreatedEvent(entity));

        await _context.Admins.AddAsync(entity, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
