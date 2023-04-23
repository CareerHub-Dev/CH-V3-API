using Application.Common.Entensions;
using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.JobDirections.Commands.CreateJobDirection;

public class CreateJobDirectionCommandValidator
    : AbstractValidator<CreateJobDirectionCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateJobDirectionCommandValidator(
        IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(32)
            .MustAsync(BeUniqueName).WithMessage("The specified name already exists.");
    }

    private async Task<bool> BeUniqueName(
        string name,
        CancellationToken cancellationToken)
    {
        return !await _context.JobDirections
            .AnyAsync(x => x.Name.Trim().ToLower() == name.NormalizeName(), cancellationToken);
    }
}
