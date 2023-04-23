using Application.Common.DTO.JobPositions;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobPositions.Queries.GetBriefJobPositions;

public record GetJobPositionsQuery
    : IRequest<IEnumerable<JobPositionDTO>>
{
    public Guid JobDirectionId { get; init; }
    public string SearchTerm { get; init; } = string.Empty;
}

public class GetJobPositionsQueryHandler
    : IRequestHandler<GetJobPositionsQuery, IEnumerable<JobPositionDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetJobPositionsQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<JobPositionDTO>> Handle(
        GetJobPositionsQuery request,
        CancellationToken cancellationToken)
    {
        if (!await _context.JobDirections
            .AnyAsync(x => x.Id == request.JobDirectionId))
        {
            throw new NotFoundException(nameof(JobDirection), request.JobDirectionId);
        }

        return await _context.JobPositions
            .Where(x => x.JobDirectionId == request.JobDirectionId)
            .Search(request.SearchTerm)
            .OrderBy(x => x.Name)
            .MapToJobPositionDTO()
            .ToListAsync();
    }
}