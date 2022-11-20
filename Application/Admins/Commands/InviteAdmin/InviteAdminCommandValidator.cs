using Application.Common.Entensions;
using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Admins.Commands.InviteAdmin;

public class InviteAdminCommandValidator
    : AbstractValidator<InviteAdminCommand>
{
    private readonly IApplicationDbContext _context;

    public InviteAdminCommandValidator(
        IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(256)
            .MustAsync(BeUniqueEmail).WithMessage("The specified email already exists.");
    }

    private async Task<bool> BeUniqueEmail(
        string email,
        CancellationToken cancellationToken)
    {
        var query = _context.Accounts.Select(x => x.NormalizedEmail)
            .Union(_context.StudentLogs.Select(x => x.NormalizedEmail));

        return !await query
            .AnyAsync(x => x == email.NormalizeName(), cancellationToken);
    }
}
