using Application.Common.DTO.Posts;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Posts.Queries.GetPost;

public record GetPostQuery
    : IRequest<PostDTO>
{
    public Guid PostId { get; init; }
    public bool IsAccountMustBeVerified { get; init; }
}

public class GetPostQueryHandler
    : IRequestHandler<GetPostQuery, PostDTO>
{
    private readonly IApplicationDbContext _context;

    public GetPostQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PostDTO> Handle(
        GetPostQuery request,
        CancellationToken cancellationToken)
    {
        var post = await _context.Posts
            .AsNoTracking()
            .Include(x => x.Account)
            .FirstOrDefaultAsync(x => x.Id == request.PostId);

        if (post == null)
        {
            throw new NotFoundException(nameof(Post), request.PostId);
        }

        return new PostDTO
        {
            Id = post.Id,
            Text = post.Text,
            Images = post.Images,
            CreatedDate = post.CreatedDate,
            Account = post.Account!.MapToAccountOfPostDTO()
        };
    }
}