using Application.Common.Entensions;
using Application.Common.Interfaces;
using Application.JobPositions.Commands.UpdateJobPosition;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.JobPositions.Commands.CreateJobPosition;

public class CreateJobPositionCommandValidator
    : AbstractValidator<CreateJobPositionCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateJobPositionCommandValidator(
        IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(32);
    }
}
