using Application.Common.DTO.JobPositions;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobPositions.Queries.GetBriefJobPosition;

public record GetJobPositionQuery(Guid JobPositionId)
    : IRequest<JobPositionDTO>;

public class GetJobPositionQueryHandler
    : IRequestHandler<GetJobPositionQuery, JobPositionDTO>
{
    private readonly IApplicationDbContext _context;

    public GetJobPositionQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<JobPositionDTO> Handle(
        GetJobPositionQuery request,
        CancellationToken cancellationToken)
    {
        var tag = await _context.JobPositions
            .MapToJobPositionDTO()
            .FirstOrDefaultAsync(x => x.Id == request.JobPositionId);

        if (tag == null)
        {
            throw new NotFoundException(nameof(JobPosition), request.JobPositionId);
        }

        return tag;
    }
}
