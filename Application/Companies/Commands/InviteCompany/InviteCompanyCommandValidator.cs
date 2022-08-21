using Application.Common.Entensions;
using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Commands.InviteCompany;

public class InviteCompanyCommandValidator : AbstractValidator<InviteCompanyCommand>
{
    private readonly IApplicationDbContext _context;

    public InviteCompanyCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MustAsync(BeUniqueEmail).WithMessage("The specified email already exists.");
    }

    private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
    {
        var query = _context.Accounts.Select(x => x.NormalizedEmail)
            .Union(_context.StudentLogs.Select(x => x.NormalizedEmail));

        return !await query
            .AnyAsync(x => x == email.NormalizeName(), cancellationToken);
    }
}
