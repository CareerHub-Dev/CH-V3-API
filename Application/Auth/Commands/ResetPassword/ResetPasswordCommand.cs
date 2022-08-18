using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Auth.Commands.ResetPassword;

public record ResetPasswordCommand : IRequest
{
    public string Token { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
{
    private readonly IApplicationDbContext _context;

    public ResetPasswordCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts
                .SingleOrDefaultAsync(x => x.ResetToken == request.Token, cancellationToken);

        if (account == null)
        {
            throw new NotFoundException(nameof(Account), request.Token);
        }

        if (account.IsResetTokenExpired)
        {
            throw new ArgumentException("Token is expired.");
        }

        account.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        account.ResetToken = null;
        account.ResetTokenExpires = null;
        account.PasswordReset = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
