using Application.Common.Models.Authentication;

namespace Application.Common.Interfaces;

public interface ISecurityService
{
    Task<AuthenticateResult> AuthenticateAsync(string email, string password, string ipAddress, CancellationToken cancellationToken = default);
}
