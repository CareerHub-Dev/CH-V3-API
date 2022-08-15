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
        return !await _context.Accounts.AnyAsync(x => x.NormalizedEmail == email.NormalizeName(), cancellationToken);
    }
}
