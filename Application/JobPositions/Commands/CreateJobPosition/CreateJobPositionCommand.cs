using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobPositions.Commands.CreateJobPosition;

public record CreateJobPositionCommand
    : IRequest<Guid>
{
    public string Name { get; init; } = string.Empty;
    public Guid JobDirectionId { get; init; }
}

public class CreateJobPositionCommandHandler
    : IRequestHandler<CreateJobPositionCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateJobPositionCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(
        CreateJobPositionCommand request,
        CancellationToken cancellationToken)
    {
        if (!await _context.JobDirections
            .AnyAsync(x => x.Id == request.JobDirectionId))
        {
            throw new NotFoundException(nameof(JobDirection), request.JobDirectionId);
        }

        var jobPosition = new JobPosition
        {
            Name = request.Name,
            JobDirectionId = request.JobDirectionId
        };

        await _context.JobPositions.AddAsync(jobPosition);

        await _context.SaveChangesAsync();

        return jobPosition.Id;
    }
}
