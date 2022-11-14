using Application.Common.Entensions;
using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Tags.Commands.UpdateTag;

public class UpdateTagCommandValidator
    : AbstractValidator<UpdateTagCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateTagCommandValidator(
        IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(32)
            .MustAsync(BeUniqueName).WithMessage("The specified name already exists.");
    }

    private async Task<bool> BeUniqueName(
        UpdateTagCommand model, 
        string name, 
        CancellationToken cancellationToken)
    {
        return !await _context.Tags
            .Where(x => x.Id != model.TagId)
            .AnyAsync(x => x.Name.Trim().ToLower() == name.NormalizeName(), cancellationToken);
    }
}
