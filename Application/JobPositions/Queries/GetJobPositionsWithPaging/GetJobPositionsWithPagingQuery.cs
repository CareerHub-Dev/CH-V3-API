using Application.Common.DTO.JobPositions;
using Application.Common.Entensions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using MediatR;

namespace Application.JobPositions.Queries.GetJobPositions;

public record GetJobPositionsWithPagingQuery
    : IRequest<PaginatedList<JobPositionDTO>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string SearchTerm { get; init; } = string.Empty;

    public string OrderByExpression { get; init; } = string.Empty;
}

public class GetJobPositionsWithPagingQueryHandler
    : IRequestHandler<GetJobPositionsWithPagingQuery, PaginatedList<JobPositionDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetJobPositionsWithPagingQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<JobPositionDTO>> Handle(
        GetJobPositionsWithPagingQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.JobPositions
            .Search(request.SearchTerm)
            .MapToJobPositionDTO()
            .OrderByExpression(request.OrderByExpression)
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}