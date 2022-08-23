using Application.Services.Jwt.Models;
using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IJwtService
{
    JwtToken GenerateJwtToken(Guid accountId);
    Task<Guid?> ValidateJwtTokenAsync(string token);
    Task<RefreshToken> GenerateRefreshTokenAsync(string ipAddress, CancellationToken cancellationToken = default);
}
