using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobDirections.Commands.UpdateJobDirection;

public record UpdateJobDirectionCommand
    : IRequest
{
    public Guid JobDirectionId { get; init; }
    public string Name { get; init; } = string.Empty;
    public TemplateLanguage RecomendedTemplateLanguage { get; init; }
}

public class UpdateJobDirectionCommandHandler
    : IRequestHandler<UpdateJobDirectionCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateJobDirectionCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        UpdateJobDirectionCommand request,
        CancellationToken cancellationToken)
    {
        var jobDirection = await _context.JobDirections
            .FirstOrDefaultAsync(x => x.Id == request.JobDirectionId);

        if (jobDirection == null)
        {
            throw new NotFoundException(nameof(JobDirection), request.JobDirectionId);
        }

        jobDirection.Name = request.Name;
        jobDirection.RecomendedTemplateLanguage = request.RecomendedTemplateLanguage;

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}
