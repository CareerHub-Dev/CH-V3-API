using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.Commands.RevokeRefreshToken;

public record RevokeRefreshTokenCommand : IRequest
{
    public string Token { init; get; } = string.Empty;
}
public class RevokeTokenCommandHandler : IRequestHandler<RevokeRefreshTokenCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IСurrentRemoteIpAddressService _сurrentRemoteIpAddressService;
    public readonly IRefreshTokenHelper _refreshTokenHelper;
    public RevokeTokenCommandHandler(
        IApplicationDbContext context, 
        IСurrentRemoteIpAddressService сurrentRemoteIpAddressService, 
        IRefreshTokenHelper refreshTokenHelper)
    {
        _context = context;
        _сurrentRemoteIpAddressService = сurrentRemoteIpAddressService;
        _refreshTokenHelper = refreshTokenHelper;
    }

    public async Task<Unit> Handle(RevokeRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await _context.RefreshTokens
            .SingleOrDefaultAsync(x => x.Token == request.Token);

        if (refreshToken == null)
        {
            throw new NotFoundException(nameof(RefreshToken), request.Token);
        }

        if (!refreshToken.IsActive)
        {
            throw new ArgumentException("Token is not active.");
        }
        
        _refreshTokenHelper.RevokeRefreshToken(refreshToken, _сurrentRemoteIpAddressService.IpAddress, "Revoked without replacement");

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}