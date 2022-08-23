﻿using Application.Common.Entensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Authentication;

namespace Application.Accounts.Commands.Authenticate;

public record AuthenticateCommand : IRequest<AuthenticateResponse>
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string IpAddress { get; init; } = string.Empty;
}

public class AuthenticateCommandHandler : IRequestHandler<AuthenticateCommand, AuthenticateResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IJwtService _jwtService;
    private readonly AppSettings _appSettings;

    public AuthenticateCommandHandler(IApplicationDbContext context, IJwtService jwtService, IOptions<AppSettings> appSettings)
    {
        _context = context;
        _jwtService = jwtService;
        _appSettings = appSettings.Value;
    }

    public async Task<AuthenticateResponse> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts
                .Include(x => x.RefreshTokens)
                .SingleOrDefaultAsync(x =>
                    x.NormalizedEmail == request.Email.NormalizeName(),
                cancellationToken);

        if (account == null || !account.IsVerified || !BCrypt.Net.BCrypt.Verify(request.Password, account.PasswordHash))
        {
            throw new AuthenticationException("This combination of email and password doesn't exist");
        }

        var jwtToken = _jwtService.GenerateJwtToken(account.Id);
        var refreshToken = await _jwtService.GenerateRefreshTokenAsync(request.IpAddress, cancellationToken);
        account.RefreshTokens.Add(refreshToken);

        account.RefreshTokens.RemoveAll(x =>
            !x.IsActive &&
            x.Created.AddDays(_appSettings.RefreshTokenTTL) <= DateTime.UtcNow);

        await _context.SaveChangesAsync(cancellationToken);

        return new AuthenticateResponse
        {
            JwtToken = jwtToken.Token,
            JwtTokenExpires = jwtToken.Expires,
            RefreshToken = refreshToken.Token,
            AccountId = account.Id,
            Role = account.GetType().Name
        };
    }
}
