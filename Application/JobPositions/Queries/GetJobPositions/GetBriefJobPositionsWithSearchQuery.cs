using Application.Common.DTO.JobPositions;
using Application.Common.Entensions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobPositions.Queries.GetJobPositions;

public record GetBriefJobPositionsWithSearchQuery : IRequest<IList<BriefJobPositionDTO>>
{
    public string SearchTerm { get; init; } = string.Empty;
}

public class GetBriefJobPositionsWithSearchQueryHandler : IRequestHandler<GetBriefJobPositionsWithSearchQuery, IList<BriefJobPositionDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetBriefJobPositionsWithSearchQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IList<BriefJobPositionDTO>> Handle(GetBriefJobPositionsWithSearchQuery request, CancellationToken cancellationToken)
    {
        return await _context.JobPositions
            .AsNoTracking()
            .Search(request.SearchTerm)
            .OrderBy(x => x.Name)
            .Select(x => new BriefJobPositionDTO
            {
                Id = x.Id,
                Name = x.Name,
            })
            .ToListAsync();
    }
}