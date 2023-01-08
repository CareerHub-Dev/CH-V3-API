using Application.Common.DTO.Posts;
using Application.Common.Entensions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Posts.Queries.GetPostsOfAccountWithPaging;

public record GetPostsOfAccountWithPaging
    : IRequest<PaginatedList<PostDTO>>
{
    public Guid AccountId { get; init; }
    public bool? IsAccountMustBeVerified { get; init; }

    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string SearchTerm { get; init; } = string.Empty;

    public string OrderByExpression { get; init; } = string.Empty;
}

public class GetPostsOfAccountWithPagingHandler
    : IRequestHandler<GetPostsOfAccountWithPaging, PaginatedList<PostDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetPostsOfAccountWithPagingHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<PostDTO>> Handle(
        GetPostsOfAccountWithPaging request, 
        CancellationToken cancellationToken)
    {
        var posts = await _context.Posts
            .AsNoTracking()
            .Include(x => x.Account)
            .Where(x => x.AccountId == request.AccountId)
            .Filter(isAccountVerified: request.IsAccountMustBeVerified)
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