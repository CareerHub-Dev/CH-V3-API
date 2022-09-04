using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Admins.Commands.UpdateAdmin;

public record UpdateAdminCommand : IRequest
{
    public Guid AdminId { get; init; }
    public bool IsSuperAdmin { get; init; }
}

public class UpdateAdminCommandHandler : IRequestHandler<UpdateAdminCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateAdminCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateAdminCommand request, CancellationToken cancellationToken)
    {
        var admin = await _context.Admins
            .FirstOrDefaultAsync(x => x.Id == request.AdminId);

        if (admin == null)
        {
            throw new NotFoundException(nameof(Admin), request.AdminId);
        }

        admin.IsSuperAdmin = request.IsSuperAdmin;

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}