using Application.Common.DTO.Posts;
using Application.Common.Entensions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Posts.Queries.GetPostsWithPaging;

public record GetPostsWithPagingQuery
    : IRequest<PaginatedList<PostDTO>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public Guid? AccountId { get; init; }
    public bool? IsAccountMustBeVerified { get; init; }

    public string OrderByExpression { get; init; } = string.Empty;
}

public class GetPostsWithPagingQueryHandler 
    : IRequestHandler<GetPostsWithPagingQuery, PaginatedList<PostDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetPostsWithPagingQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<PostDTO>> Handle(
        GetPostsWithPagingQuery request, 
        CancellationToken cancellationToken)
    {
        var posts = await _context.Posts
            .AsNoTracking()
            .Include(x => x.Account)
            .Filter(
                isAccountVerified: request.IsAccountMustBeVerified, 
                accountId: request.AccountId
            )
            .ToPagedListAsync(request.PageNumber, request.PageSize);

        var mapped = posts.Select(x => new PostDTO
        {
            Id = x.Id,
            Text = x.Text,
            Images = x.Images,
            CreatedDate = x.CreatedDate,
            Account = x.Account!.MapToAccountOfPostDTO()
        }).ToList();


        return new PaginatedList<PostDTO>(
            mapped,
            posts.MetaData);
    }
}