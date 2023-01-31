using Application.Common.DTO.Posts;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Posts.Queries.GetPostOfAccount;

public record GetPostOfAccountQuery
    : IRequest<PostDTO>
{
    public Guid PostId { get; init; }

    public Guid AccountId { get; init; }
    public bool? IsAccountMustBeVerified { get; init; }
}

public class GetPostOfAccountQueryHandler
    : IRequestHandler<GetPostOfAccountQuery, PostDTO>
{
    private readonly IApplicationDbContext _context;

    public GetPostOfAccountQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PostDTO> Handle(
        GetPostOfAccountQuery request,
        CancellationToken cancellationToken)
    {
        var post = await _context.Posts
            .AsNoTracking()
            .Include(x => x.Account)
            .Filter(isAccountVerified: request.IsAccountMustBeVerified, accountIds: new List<Guid> { request.AccountId })
            .MapToPostDTO()
            .FirstOrDefaultAsync(x => x.Id == request.PostId);

        if (post == null)
        {
            throw new NotFoundException(nameof(Post), request.PostId);
        }

        return post;
    }
}
