using Application.Common.DTO.JobDirection;
using Application.Common.DTO.JobPositions;
using Application.Common.Entensions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobDirections.Queries.GetJobDirections;

public record GetJobDirectionsQuery
    : IRequest<IEnumerable<JobDirectionDTO>>
{
    public string SearchTerm { get; init; } = string.Empty;
}

public class GetJobDirectionsQueryHandler
    : IRequestHandler<GetJobDirectionsQuery, IEnumerable<JobDirectionDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetJobDirectionsQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<JobDirectionDTO>> Handle(
        GetJobDirectionsQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.JobDirections
            .Search(request.SearchTerm)
            .OrderBy(x => x.Name)
            .MapToJobDirectionDTO()
            .ToListAsync();
    }
}