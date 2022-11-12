using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Bans.Commands.UpdateBan;

public record UpdateBanCommand : IRequest
{
    public Guid BanId { get; init; }
    public string Reason { get; init; } = string.Empty;
    public DateTime Expires { get; init; }
}

public class UpdateBanCommandHandler : IRequestHandler<UpdateBanCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateBanCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateBanCommand request, CancellationToken cancellationToken)
    {
        var ban = await _context.Bans
            .FirstOrDefaultAsync(x => x.Id == request.BanId);

        if (ban == null)
        {
            throw new NotFoundException(nameof(Ban), request.BanId);
        }

        ban.Reason = request.Reason;
        ban.Expires = request.Expires;

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}
