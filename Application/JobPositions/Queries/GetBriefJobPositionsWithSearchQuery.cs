using Application.Common.Entensions;
using Application.Common.Interfaces;
using Application.Common.Models.JobPosition;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobPositions.Queries;

public record GetBriefJobPositionsWithSearchQuery : IRequest<IList<BriefJobPositionDTO>>
{
    public string? SearchTerm { get; init; }
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
            .Search(request.SearchTerm ?? "")
            .OrderBy(x => x.Name)
            .Select(x => new BriefJobPositionDTO
            {
                Id = x.Id,
                Name = x.Name,
            })
            .ToListAsync();
    }
}