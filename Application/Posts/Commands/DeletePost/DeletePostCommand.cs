using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Posts.Commands.DeletePost;

public record DeletePostCommand(Guid PostId) : IRequest;

public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand>
{
    private readonly IApplicationDbContext _context;

    public DeletePostCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var post = await _context.Posts
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.PostId);

        if (post == null)
        {
            throw new NotFoundException(nameof(Post), request.PostId);
        }

        _context.Posts.Remove(post);

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}