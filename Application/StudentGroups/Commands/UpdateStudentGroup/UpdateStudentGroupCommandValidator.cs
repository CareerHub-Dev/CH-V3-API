using Application.Common.Entensions;
using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.StudentGroups.Commands.UpdateStudentGroup;

public class UpdateStudentGroupCommandValidator
    : AbstractValidator<UpdateStudentGroupCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateStudentGroupCommandValidator(
        IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(32)
            .MustAsync(BeUniqueName).WithMessage("The specified name already exists.");
    }

    private async Task<bool> BeUniqueName(
        UpdateStudentGroupCommand model,
        string name,
        CancellationToken cancellationToken)
    {
        return !await _context.StudentGroups
            .Where(x => x.Id != model.StudentGroupId)
            .AnyAsync(x => x.Name.Trim().ToLower() == name.NormalizeName(), cancellationToken);
    }
}
