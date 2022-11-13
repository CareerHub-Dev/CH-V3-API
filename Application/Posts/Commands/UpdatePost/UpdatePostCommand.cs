using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Posts.Commands.UpdatePost;

public record UpdatePostCommand : IRequest
{
    public Guid PostId { get; init; }
    public string Text { get; init; } = string.Empty;
}

public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdatePostCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        var post = await _context.Posts
            .FirstOrDefaultAsync(x => x.Id == request.PostId);

        if (post == null)
        {
            throw new NotFoundException(nameof(Post), request.PostId);
        }

        post.Text = request.Text;

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}