using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.JobPositions.Commands.CreateJobPosition;

public record CreateJobPositionCommand : IRequest<Guid>
{
    public string Name { get; init; } = string.Empty;
}

public class CreateJobPositionCommandHandler : IRequestHandler<CreateJobPositionCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateJobPositionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateJobPositionCommand request, CancellationToken cancellationToken)
    {
        var jobPosition = new JobPosition
        {
            Name = request.Name,
        };

        await _context.JobPositions.AddAsync(jobPosition);

        await _context.SaveChangesAsync();

        return jobPosition.Id;
    }
}
