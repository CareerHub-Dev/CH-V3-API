using Application.Common.Entensions;
using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Admins.Commands.InviteAdmin;

public class InviteAdminCommandValidator : AbstractValidator<InviteAdminCommand>
{
    private readonly IApplicationDbContext _context;

    public InviteAdminCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MustAsync(BeUniqueEmail).WithMessage("The specified email already exists.");
    }

    private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
    {
        return !await _context.Accounts.AnyAsync(x => x.NormalizedEmail == email.NormalizeName(), cancellationToken);
    }
}
