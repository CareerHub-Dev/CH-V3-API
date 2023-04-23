using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.JobDirections.Commands.CreateJobDirection;

public record CreateJobDirectionCommand
    : IRequest<Guid>
{
    public string Name { get; init; } = string.Empty;
    public TemplateLanguage RecomendedTemplateLanguage { get; init; }
}

public class CreateJobDirectionCommandHandler
    : IRequestHandler<CreateJobDirectionCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateJobDirectionCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(
        CreateJobDirectionCommand request,
        CancellationToken cancellationToken)
    {
        var jobDirection = new JobDirection
        {
            Name = request.Name,
            RecomendedTemplateLanguage = request.RecomendedTemplateLanguage
        };

        await _context.JobDirections.AddAsync(jobDirection);

        await _context.SaveChangesAsync();

        return jobDirection.Id;
    }
}
