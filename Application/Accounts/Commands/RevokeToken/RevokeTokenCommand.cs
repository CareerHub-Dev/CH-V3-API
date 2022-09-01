using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.Commands.RevokeToken;

public record RevokeTokenCommand : IRequest
{
    public string Token { init; get; } = string.Empty;
    public string IpAddress { init; get; } = string.Empty;
}
public class RevokeTokenCommandHandler : IRequestHandler<RevokeTokenCommand>
{
    private readonly IApplicationDbContext _context;

    public RevokeTokenCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await _context.RefreshTokens
                .SingleOrDefaultAsync(x => x.Token == request.Token);

        if (refreshToken == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.RefreshToken), request.Token);
        }

        if (!refreshToken.IsActive)
        {
            throw new ArgumentException("Token is not active.");
        }

        refreshToken.Revoked = DateTime.UtcNow;
        refreshToken.RevokedByIp = request.IpAddress;
        refreshToken.ReasonRevoked = "Revoked without replacement";

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}