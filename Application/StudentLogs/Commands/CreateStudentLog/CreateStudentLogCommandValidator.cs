using Application.Common.Entensions;
using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.StudentLogs.Commands.CreateStudentLog;

public class CreateStudentLogCommandValidator
    : AbstractValidator<CreateStudentLogCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateStudentLogCommandValidator(
        IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Email)
            .NotEmpty()
            .NureEmailAddress()
            .MaximumLength(256)
            .MustAsync(BeUniqueEmail).WithMessage("The specified email already exists.");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(32);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(32);
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
