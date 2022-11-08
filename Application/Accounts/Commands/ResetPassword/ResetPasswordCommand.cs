using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.Commands.ResetPassword;

public record ResetPasswordCommand : IRequest
{
    public string Token { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher<Account> _passwordHasher;

    public ResetPasswordCommandHandler(IApplicationDbContext context, IPasswordHasher<Account> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts
            .SingleOrDefaultAsync(x => x.ResetToken == request.Token);

        if (account == null)
        {
            throw new NotFoundException(nameof(Account), request.Token);
        }

        if (account.IsResetTokenExpired)
        {
            throw new ArgumentException("Token is expired.");
        }

        account.PasswordHash = _passwordHasher.HashPassword(account, request.Password);
        account.ResetToken = null;
        account.ResetTokenExpires = null;
        account.PasswordReset = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}
