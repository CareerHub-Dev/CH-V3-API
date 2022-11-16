using Application.Admins.Events;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Admins.Commands.InviteAdmin;

public record InviteAdminCommand 
    : IRequest<Guid>
{
    public string Email { get; init; } = string.Empty;
}

public class InviteAdminCommandHandler 
    : IRequestHandler<InviteAdminCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IPublisher _publisher;

    public InviteAdminCommandHandler(
        IApplicationDbContext context, 
        IPublisher publisher)
    {
        _context = context;
        _publisher = publisher;
    }

    public async Task<Guid> Handle(
        InviteAdminCommand request, 
        CancellationToken cancellationToken)
    {
        var admin = new Admin
        {
            Email = request.Email,
            IsSuperAdmin = false
        };

        await _context.Admins.AddAsync(admin);

        await _context.SaveChangesAsync();

        await _publisher.Publish(new AdminInvitedEvent(admin));

        return admin.Id;
    }
}
