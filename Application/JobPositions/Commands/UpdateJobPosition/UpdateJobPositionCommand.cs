using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobPositions.Commands.UpdateJobPosition;

public record UpdateJobPositionCommand
    : IRequest
{
    public Guid JobPositionId { get; init; }
    public string Name { get; init; } = string.Empty;
}

public class UpdateJobPositionCommandHandler
    : IRequestHandler<UpdateJobPositionCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateJobPositionCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        UpdateJobPositionCommand request,
        CancellationToken cancellationToken)
    {
        var jobPosition = await _context.JobPositions
            .Where(x => x.Id == request.JobPositionId)
            .FirstOrDefaultAsync();

        if (jobPosition == null)
        {
            throw new NotFoundException(nameof(JobPosition), request.JobPositionId);
        }

        jobPosition.Name = request.Name;

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}
