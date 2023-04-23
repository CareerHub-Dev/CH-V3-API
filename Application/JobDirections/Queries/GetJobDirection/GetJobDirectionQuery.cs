using Application.Common.DTO.JobDirection;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobDirections.Queries.GetJobDirection;

public record GetJobDirectionQuery(Guid JobDirectionId)
    : IRequest<JobDirectionDTO>;

public class GetJobDirectionQueryHandler
    : IRequestHandler<GetJobDirectionQuery, JobDirectionDTO>
{
    private readonly IApplicationDbContext _context;

    public GetJobDirectionQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<JobDirectionDTO> Handle(
        GetJobDirectionQuery request,
        CancellationToken cancellationToken)
    {
        var jobDirection = await _context.JobDirections
            .MapToJobDirectionDTO()
            .FirstOrDefaultAsync(x => x.Id == request.JobDirectionId);

        if (jobDirection == null)
        {
            throw new NotFoundException(nameof(JobDirection), request.JobDirectionId);
        }

        return jobDirection;
    }
}
