using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.Commands.RevokeToken;

public record RevokeTokenCommand : IRequest
{
    public string Token { init; get; } = string.Empty;
    public string IpAddress { init; get; } = string.Empty;
    public Guid AccountId { init; get; }
}
public class RevokeTokenCommandHandler : IRequestHandler<RevokeTokenCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IJwtService _jwtService;

    public RevokeTokenCommandHandler(IApplicationDbContext context, IJwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    public async Task<Unit> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await _context.RefreshTokens
                .SingleOrDefaultAsync(x => x.Token == request.Token && x.AccountId == request.AccountId);

        if (refreshToken == null || !refreshToken.IsActive)
        {
            throw new ArgumentException("Token does not exist or is not active");
        }

        refreshToken.Revoked = DateTime.UtcNow;
        refreshToken.RevokedByIp = request.IpAddress;
        refreshToken.ReasonRevoked = "Revoked without replacement";

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}