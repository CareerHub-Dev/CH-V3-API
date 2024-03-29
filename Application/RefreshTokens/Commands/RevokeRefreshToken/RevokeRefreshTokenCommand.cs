﻿using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.RefreshTokens.Commands.RevokeRefreshToken;

public record RevokeRefreshTokenCommand
    : IRequest
{
    public string Token { init; get; } = string.Empty;
}
public class RevokeRefreshTokenCommandHandler
    : IRequestHandler<RevokeRefreshTokenCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IСurrentRemoteIpAddressService _сurrentRemoteIpAddressService;
    public RevokeRefreshTokenCommandHandler(
        IApplicationDbContext context,
        IСurrentRemoteIpAddressService сurrentRemoteIpAddressService)
    {
        _context = context;
        _сurrentRemoteIpAddressService = сurrentRemoteIpAddressService;
    }

    public async Task<Unit> Handle(
        RevokeRefreshTokenCommand request,
        CancellationToken cancellationToken)
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

        refreshToken.Revoked = DateTime.UtcNow;
        refreshToken.RevokedByIp = _сurrentRemoteIpAddressService.IpAddress;
        refreshToken.ReasonRevoked = "Revoked without replacement";

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}