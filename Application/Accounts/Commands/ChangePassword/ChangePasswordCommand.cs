using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.Commands.ChangePassword;

public record ChangePasswordCommand : IRequest
{
    public Guid AccountId { get; set; }
    public string OldPassword { get; init; } = string.Empty;
    public string NewPassword { get; init; } = string.Empty;
}

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher<Account> _passwordHasher;

    public ChangePasswordCommandHandler(IApplicationDbContext context, IPasswordHasher<Account> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts
            .FirstOrDefaultAsync(x => x.Id == request.AccountId);

        if (account == null)
        {
            throw new NotFoundException(nameof(Account), request.AccountId);
        }

        var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(account, account.PasswordHash, request.OldPassword);

        switch (passwordVerificationResult)
        {
            case PasswordVerificationResult.Failed:
                throw new ArgumentException("The old password does not match.");

            case PasswordVerificationResult.SuccessRehashNeeded or PasswordVerificationResult.Success:
                account.PasswordHash = _passwordHasher.HashPassword(account, request.NewPassword);
                break;
        }

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}