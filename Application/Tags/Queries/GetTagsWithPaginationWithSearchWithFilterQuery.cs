using Application.Common.Entensions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Application.Tags.Queries.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Tags.Queries;

public record GetTagsWithPaginationWithSearchWithFilterQuery : IRequest<PaginatedList<TagDTO>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public bool? IsAccepted { get; init; }

    public string? SearchTerm { get; init; }
}

public class GetTagsWithPaginationWithSearchWithFilterQueryHandler : IRequestHandler<GetTagsWithPaginationWithSearchWithFilterQuery, PaginatedList<TagDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetTagsWithPaginationWithSearchWithFilterQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<TagDTO>> Handle(GetTagsWithPaginationWithSearchWithFilterQuery request, CancellationToken cancellationToken)
    {
        return await _context.Tags
            .AsNoTracking()
            .Search(request.SearchTerm ?? "")
            .Filter(isAccepted: request.IsAccepted)
            .OrderBy(x => x.Name)
            .Select(x => new TagDTO
            {
                Id = x.Id,
                Name = x.Name,
                IsAccepted = x.IsAccepted,
                Created = x.Created,
                CreatedBy = x.CreatedBy,
                LastModified = x.LastModified,
                LastModifiedBy = x.LastModifiedBy,
            })
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}