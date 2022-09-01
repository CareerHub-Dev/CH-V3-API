using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Admins.Commands.DeleteAdmin;

public record DeleteAdminCommand(Guid AdminId) : IRequest;

public class DeleteAdminCommandHandler : IRequestHandler<DeleteAdminCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteAdminCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteAdminCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Admins
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.AdminId);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Admin), request.AdminId);
        }

        _context.Admins.Remove(entity);

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}