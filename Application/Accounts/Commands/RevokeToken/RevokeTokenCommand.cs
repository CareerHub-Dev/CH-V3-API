using Application.Common.Exceptions;
using Application.Common.Helpers;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.Commands.RevokeToken;

public record RevokeTokenCommand : IRequest
{
    public string Token { init; get; } = string.Empty;
}
public class RevokeTokenCommandHandler : IRequestHandler<RevokeTokenCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IСurrentRemoteIpAddressService _сurrentRemoteIpAddressService;

    public RevokeTokenCommandHandler(IApplicationDbContext context, IСurrentRemoteIpAddressService сurrentRemoteIpAddressService)
    {
        _context = context;
        _сurrentRemoteIpAddressService = сurrentRemoteIpAddressService;
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

        RefreshTokenHelper.RevokeRefreshToken(refreshToken, _сurrentRemoteIpAddressService.IpAddress, "Revoked without replacement");

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}