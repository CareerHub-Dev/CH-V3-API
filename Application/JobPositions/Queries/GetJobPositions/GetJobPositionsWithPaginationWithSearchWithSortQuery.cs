using Application.Common.DTO.JobPositions;
using Application.Common.Entensions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobPositions.Queries.GetJobPositions;

public record GetJobPositionsWithPaginationWithSearchWithSortQuery : IRequest<PaginatedList<JobPositionDTO>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string SearchTerm { get; init; } = string.Empty;

    public string OrderByExpression { get; init; } = string.Empty;
}

public class GetJobPositionsWithPaginationWithSearchWithSortQueryHandler : IRequestHandler<GetJobPositionsWithPaginationWithSearchWithSortQuery, PaginatedList<JobPositionDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetJobPositionsWithPaginationWithSearchWithSortQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<JobPositionDTO>> Handle(GetJobPositionsWithPaginationWithSearchWithSortQuery request, CancellationToken cancellationToken)
    {
        return await _context.JobPositions
            .AsNoTracking()
            .Search(request.SearchTerm)
            .MapToJobPositionDTO()
            .OrderByExpression(request.OrderByExpression)
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}