using Application.Common.Entensions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Application.JobPositions.Queries.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobPositions.Queries;

public record GetJobPositionsWithPaginationWithSearchQuery : IRequest<PaginatedList<JobPositionDTO>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string? SearchTerm { get; init; }
}

public class GetJobPositionsWithPaginationWithSearchQueryHandler : IRequestHandler<GetJobPositionsWithPaginationWithSearchQuery, PaginatedList<JobPositionDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetJobPositionsWithPaginationWithSearchQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<JobPositionDTO>> Handle(GetJobPositionsWithPaginationWithSearchQuery request, CancellationToken cancellationToken)
    {
        return await _context.JobPositions
            .AsNoTracking()
            .Search(request.SearchTerm ?? "")
            .OrderByDescending(x => x.Name)
            .Select(x => new JobPositionDTO
            {
                Id = x.Id,
                Name = x.Name,
                Created = x.Created,
                CreatedBy = x.CreatedBy,
                LastModified = x.LastModified,
                LastModifiedBy = x.LastModifiedBy,
            })
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}