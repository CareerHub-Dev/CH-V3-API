using Application.Common.DTO.Posts;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Posts.Queries.GetPostsOfAccountWithPaging;

public record GetPostsOfAccountWithPagingQuery
    : IRequest<PaginatedList<PostDTO>>
{
    public Guid AccountId { get; init; }
    public bool? IsAccountMustBeVerified { get; init; }

    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string SearchTerm { get; init; } = string.Empty;

    public string OrderByExpression { get; init; } = string.Empty;
}

public class GetPostsOfAccountWithPagingQueryHandler
    : IRequestHandler<GetPostsOfAccountWithPagingQuery, PaginatedList<PostDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetPostsOfAccountWithPagingQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<PostDTO>> Handle(
        GetPostsOfAccountWithPagingQuery request,
        CancellationToken cancellationToken)
    {
        if (!await _context.Accounts
            .Filter(isVerified: request.IsAccountMustBeVerified)
            .AnyAsync(x => x.Id == request.AccountId))
        {
            throw new NotFoundException(nameof(Account), request.AccountId);
        }

        return await _context.Posts
            .Filter(isAccountVerified: request.IsAccountMustBeVerified, accountIds: new List<Guid> { request.AccountId })
            .MapToPostDTO()
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}