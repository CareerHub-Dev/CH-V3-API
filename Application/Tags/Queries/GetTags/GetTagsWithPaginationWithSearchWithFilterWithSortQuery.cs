using Application.Common.DTO.Tags;
using Application.Common.Entensions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Tags.Queries.GetTags;

public record GetTagsWithPaginationWithSearchWithFilterWithSortQuery : IRequest<PaginatedList<TagDTO>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public bool? IsAccepted { get; init; }

    public string SearchTerm { get; init; } = string.Empty;

    public string OrderByExpression { get; init; } = string.Empty;
}

public class GetTagsWithPaginationWithSearchWithFilterWithSortQueryHandler : IRequestHandler<GetTagsWithPaginationWithSearchWithFilterWithSortQuery, PaginatedList<TagDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetTagsWithPaginationWithSearchWithFilterWithSortQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<TagDTO>> Handle(GetTagsWithPaginationWithSearchWithFilterWithSortQuery request, CancellationToken cancellationToken)
    {
        return await _context.Tags
            .AsNoTracking()
            .Search(request.SearchTerm)
            .Filter(isAccepted: request.IsAccepted)
            .MapToTagDTO()
            .OrderByExpression(request.OrderByExpression)
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}