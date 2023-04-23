using Application.Common.Entensions;
using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.JobPositions.Commands.UpdateJobPosition;

public class UpdateJobPositionCommandValidator
    : AbstractValidator<UpdateJobPositionCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateJobPositionCommandValidator(
        IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(32)
            .MustAsync(BeUniqueName).WithMessage("The specified name already exists.");
    }

    private async Task<bool> BeUniqueName(
        UpdateJobPositionCommand model,
        string name,
        CancellationToken cancellationToken)
    {
        var jobPosition = await _context.JobPositions.FindAsync(model.JobPositionId);

        return !await _context.JobPositions
            .Where(x => x.Id != model.JobPositionId)
            .AnyAsync(x => x.Name.Trim().ToLower() == name.NormalizeName() 
            && jobPosition != null ? x.JobDirectionId == jobPosition.JobDirectionId : true, cancellationToken);
    }
}
