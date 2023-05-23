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
            .MaximumLength(32);
    }
}
