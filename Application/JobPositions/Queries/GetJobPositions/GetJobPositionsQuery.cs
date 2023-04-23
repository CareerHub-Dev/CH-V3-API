using Application.Common.DTO.JobPositions;
using Application.Common.Entensions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobPositions.Queries.GetBriefJobPositions;

public record GetJobPositionsQuery
    : IRequest<IEnumerable<JobPositionDTO>>
{
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
        return await _context.JobPositions
            .Search(request.SearchTerm)
            .OrderBy(x => x.Name)
            .MapToBriefJobPositionDTO()
            .ToListAsync();
    }
}