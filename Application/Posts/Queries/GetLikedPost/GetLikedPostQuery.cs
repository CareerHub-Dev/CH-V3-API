using Application.Common.DTO.Posts;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Posts.Queries.GetLikedPost;

public record GetLikedPostQuery
    : IRequest<LikedPostDTO>
{
    public Guid PostId { get; init; }
    public bool IsAccountMustBeVerified { get; init; }

    public Guid LikerStudentId { get; init; }
}

public class GetLikedPostQueryHandler
    : IRequestHandler<GetLikedPostQuery, LikedPostDTO>
{
    private readonly IApplicationDbContext _context;

    public GetLikedPostQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<LikedPostDTO> Handle(
        GetLikedPostQuery request,
        CancellationToken cancellationToken)
    {
        var post = await _context.Posts
            .AsNoTracking()
            .Include(x => x.Account)
            .MapToLikedPostDTO(request.LikerStudentId)
            .FirstOrDefaultAsync(x => x.Id == request.PostId);

        if (post == null)
        {
            throw new NotFoundException(nameof(Post), request.PostId);
        }

        return post;
    }
}