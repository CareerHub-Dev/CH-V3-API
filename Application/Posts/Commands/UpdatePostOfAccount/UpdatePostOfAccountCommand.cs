using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Posts.Commands.UpdatePostOfAccount;

public record UpdatePostOfAccountCommand 
    : IRequest
{
    public Guid PostId { get; init; }
    public string Text { get; init; } = string.Empty;
    public Guid AccountId { get; init; }
}

public class UpdatePostOfAccountCommandHandler 
    : IRequestHandler<UpdatePostOfAccountCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdatePostOfAccountCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        UpdatePostOfAccountCommand request, 
        CancellationToken cancellationToken)
    {
        if (!await _context.Accounts
            .AnyAsync(x => x.Id == request.AccountId))
        {
            throw new NotFoundException(nameof(Account), request.AccountId);
        }

        var post = await _context.Posts
            .FirstOrDefaultAsync(x => x.Id == request.PostId && x.AccountId == request.AccountId);

        if (post == null)
        {
            throw new NotFoundException(nameof(Post), request.PostId);
        }

        post.Text = request.Text;

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}