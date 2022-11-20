using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Posts.Commands.DeletePostOfAccount;

public record DeletePostOfAccountCommand(Guid PostId, Guid AccountId) 
    : IRequest;

public class DeletePostOfAccountCommandHandler 
    : IRequestHandler<DeletePostOfAccountCommand>
{
    private readonly IApplicationDbContext _context;

    public DeletePostOfAccountCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        DeletePostOfAccountCommand request, 
        CancellationToken cancellationToken)
    {
        if (!await _context.Accounts
            .AnyAsync(x => x.Id == request.AccountId))
        {
            throw new NotFoundException(nameof(Account), request.AccountId);
        }

        var post = await _context.Posts
            .FirstOrDefaultAsync(x => x.Id == request.PostId && x.AccoundId == request.AccountId);

        if (post == null)
        {
            throw new NotFoundException(nameof(Post), request.PostId);
        }

        _context.Posts.Remove(post);

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}