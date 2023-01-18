using Application.Common.DTO.Posts;
using Application.Common.Entensions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Posts.Queries.GetAdmininstrationPostsWithPaging;

public class GetAdmininstrationPostsWithPagingQuery
    : IRequest<PaginatedList<PostDTO>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string SearchTerm { get; init; } = string.Empty;

    public string OrderByExpression { get; init; } = string.Empty;
}

public class GetAdmininstrationPostsWithPagingQueryHandler
    : IRequestHandler<GetAdmininstrationPostsWithPagingQuery, PaginatedList<PostDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetAdmininstrationPostsWithPagingQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<PostDTO>> Handle(
        GetAdmininstrationPostsWithPagingQuery request,
        CancellationToken cancellationToken)
    {
        var adminIds = await _context.Admins.Select(x => x.Id).ToListAsync();

        return await _context.Posts
            .Filter(accountIds: adminIds)
            .MapToPostDTO()
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}