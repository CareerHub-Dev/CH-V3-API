using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobDirections.Commands.DeleteJobOffer;

public record DeleteJobDirectionCommand(Guid JobDirectionId)
    : IRequest;

public class DeleteJobDirectionCommandHandler
    : IRequestHandler<DeleteJobDirectionCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteJobDirectionCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        DeleteJobDirectionCommand request,
        CancellationToken cancellationToken)
    {
        var jobDirection = await _context.JobDirections
            .FirstOrDefaultAsync(x => x.Id == request.JobDirectionId);

        if (jobDirection == null)
        {
            throw new NotFoundException(nameof(JobDirection), request.JobDirectionId);
        }

        _context.JobDirections.Remove(jobDirection);

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}
