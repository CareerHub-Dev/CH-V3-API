﻿using Application.Common.Entensions;
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
            .MaximumLength(32)
            .MustAsync(BeUniqueName).WithMessage("The specified name already exists.");
    }

    private async Task<bool> BeUniqueName(
        CreateJobPositionCommand model,
        string name,
        CancellationToken cancellationToken)
    {
        
        return !await _context.JobPositions
            .AnyAsync(x => x.Name.Trim().ToLower() == name.NormalizeName() && x.JobDirectionId == model.JobDirectionId, cancellationToken);
    }
}
