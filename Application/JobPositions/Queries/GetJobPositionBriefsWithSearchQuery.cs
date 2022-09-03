using Application.Common.Entensions;
using Application.Common.Interfaces;
using Application.Common.Models.JobPosition;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobPositions.Queries;

public record GetJobPositionBriefsWithSearchQuery : IRequest<IEnumerable<JobPositionBriefDTO>>
{
    public string? SearchTerm { get; init; }
}

public class GetJobPositionBriefsWithSearchQueryHandler : IRequestHandler<GetJobPositionBriefsWithSearchQuery, IEnumerable<JobPositionBriefDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetJobPositionBriefsWithSearchQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<JobPositionBriefDTO>> Handle(GetJobPositionBriefsWithSearchQuery request, CancellationToken cancellationToken)
    {
        return await _context.JobPositions
            .AsNoTracking()
            .Search(request.SearchTerm ?? "")
            .OrderByDescending(x => x.Name)
            .Select(x => new JobPositionBriefDTO
            {
                Id = x.Id,
                Name = x.Name,
            })
            .ToListAsync();
    }
}