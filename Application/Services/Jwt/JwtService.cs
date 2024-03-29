﻿using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Services.Jwt.Models;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Application.Services.Jwt;

public class JwtService
    : IJwtService
{
    private readonly IApplicationDbContext _context;
    private readonly JwtSettings _jwtSettings;

    public JwtService(
        IApplicationDbContext context,
        IOptions<JwtSettings> jwtSettings)
    {
        _context = context;
        _jwtSettings = jwtSettings.Value;
    }

    public JwtToken GenerateJwtToken(Guid accountId)
    {
        var jwtToken = new JwtToken
        {
            Expires = DateTime.UtcNow.AddMinutes(60)
        };

        // generate token that is valid for 15 minutes
        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("Id", accountId.ToString()) }),
            Expires = jwtToken.Expires,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        jwtToken.Token = tokenHandler.WriteToken(token);

        return jwtToken;
    }

    public async Task<RefreshToken> GenerateRefreshTokenAsync(
        string ipAddress,
        CancellationToken cancellationToken = default)
    {
        var refreshToken = new RefreshToken
        {
            // token is a cryptographically strong random sequence of values
            Token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64)),
            // token is valid for 7 days
            Expires = DateTime.UtcNow.AddDays(7),
            Created = DateTime.UtcNow,
            CreatedByIp = ipAddress
        };

        // ensure token is unique by checking against db
        var tokenIsNotUnique = await _context.RefreshTokens
            .AnyAsync(x => x.Token == refreshToken.Token, cancellationToken);

        if (tokenIsNotUnique)
        {
            return await GenerateRefreshTokenAsync(ipAddress, cancellationToken);
        }

        return refreshToken;
    }

    public async Task<Guid?> ValidateJwtTokenAsync(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret);
        try
        {
            var result = await tokenHandler.ValidateTokenAsync(
                token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                });

            // return account id from JWT token if validation successful
            return result.Claims
                .First(x => x.Key == "Id").Value is not string id ? null : Guid.Parse(id);
        }
        catch
        {
            // return null if validation fails
            return null;
        }
    }
}
