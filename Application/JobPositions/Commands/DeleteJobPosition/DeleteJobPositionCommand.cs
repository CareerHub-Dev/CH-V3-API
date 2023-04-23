using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobPositions.Commands.DeleteJobPosition;

public record DeleteJobPositionCommand(Guid JobPositionId)
    : IRequest;

public class DeleteJobPositionCommandHandler
    : IRequestHandler<DeleteJobPositionCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteJobPositionCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        DeleteJobPositionCommand request,
        CancellationToken cancellationToken)
    {
        var jobPosition = await _context.JobPositions
            .FirstOrDefaultAsync(x => x.Id == request.JobPositionId);

        if (jobPosition == null)
        {
            throw new NotFoundException(nameof(JobPosition), request.JobPositionId);
        }

        _context.JobPositions.Remove(jobPosition);

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}
