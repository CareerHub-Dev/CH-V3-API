using Application.Common.Entensions;
using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.StudentLogs.Commands.UpdateStudentLog;

public class UpdateStudentLogCommandValidator : AbstractValidator<UpdateStudentLogCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateStudentLogCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Email)
            .NotEmpty()
            .NureEmailAddress()
            .MustAsync(BeUniqueEmail).WithMessage("The specified email already exists.");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(30);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(30);
    }

    private async Task<bool> BeUniqueEmail(UpdateStudentLogCommand model, string email, CancellationToken cancellationToken)
    {
        var query = _context.Accounts.Select(x => x.NormalizedEmail)
            .Union(_context.StudentLogs.Where(x => x.Id != model.Id).Select(x => x.NormalizedEmail));

        return !await query
            .AnyAsync(x => x == email.NormalizeName(), cancellationToken);
    }
}
