using Application.Common.Entensions;
using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.JobDirections.Commands.UpdateJobDirection;

public class UpdateJobDirectionCommandValidator
    : AbstractValidator<UpdateJobDirectionCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateJobDirectionCommandValidator(
        IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(32)
            .MustAsync(BeUniqueName).WithMessage("The specified name already exists.");
    }

    private async Task<bool> BeUniqueName(
        UpdateJobDirectionCommand model,
        string name,
        CancellationToken cancellationToken)
    {
        return !await _context.JobPositions
            .Where(x => x.Id != model.JobDirectionId)
            .AnyAsync(x => x.Name.Trim().ToLower() == name.NormalizeName(), cancellationToken);
    }
}
