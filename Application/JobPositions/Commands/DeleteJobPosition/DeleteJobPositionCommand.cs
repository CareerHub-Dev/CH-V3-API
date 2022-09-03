using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobPositions.Commands.DeleteJobPosition;

public record DeleteJobpositionCommand(Guid JobPositionId) : IRequest;

public class DeleteJobPositionCommandHandler : IRequestHandler<DeleteJobpositionCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteJobPositionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteJobpositionCommand request, CancellationToken cancellationToken)
    {
        var jobPosition = await _context.JobPositions
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.JobPositionId);

        if (jobPosition == null)
        {
            throw new NotFoundException(nameof(StudentGroup), request.JobPositionId);
        }

        _context.JobPositions.Remove(jobPosition);

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}
