using Application.Common.DTO.JobPositions;
using Application.Common.Entensions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobPositions.Queries.GetBriefJobPositions;

public record GetBriefJobPositionsQuery
    : IRequest<IEnumerable<BriefJobPositionDTO>>
{
    public string SearchTerm { get; init; } = string.Empty;
}

public class GetBriefJobPositionsQueryHandler
    : IRequestHandler<GetBriefJobPositionsQuery, IEnumerable<BriefJobPositionDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetBriefJobPositionsQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BriefJobPositionDTO>> Handle(
        GetBriefJobPositionsQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.JobPositions
            .Search(request.SearchTerm)
            .OrderBy(x => x.Name)
            .MapToBriefJobPositionDTO()
            .ToListAsync();
    }
}