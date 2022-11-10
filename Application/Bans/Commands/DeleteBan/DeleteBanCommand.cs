using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Bans.Commands.DeleteBan;

public record DeleteBanCommand(Guid BanId) : IRequest;

public class DeleteBanCommandHandler : IRequestHandler<DeleteBanCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteBanCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteBanCommand request, CancellationToken cancellationToken)
    {
        var ban = await _context.Bans
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.BanId);

        if (ban == null)
        {
            throw new NotFoundException(nameof(Ban), request.BanId);
        }

        _context.Bans.Remove(ban);

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}
