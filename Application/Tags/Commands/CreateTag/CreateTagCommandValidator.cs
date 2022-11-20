using Application.Common.Entensions;
using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Tags.Commands.CreateTag;

public class CreateTagCommandValidator
    : AbstractValidator<CreateTagCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateTagCommandValidator(
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
        return !await _context.Tags
            .AnyAsync(x => x.Name.Trim().ToLower() == name.NormalizeName(), cancellationToken);
    }
}
