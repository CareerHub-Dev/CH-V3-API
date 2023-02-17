using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.Commands.SetPayerId;

public record SetPayerIdCommand
    : IRequest
{
    public Guid AccountId { get; init; }
    public Guid PlayerId { get; init; }
}

public class SetPayerIdCommandHandler
    : IRequestHandler<SetPayerIdCommand>
{
    private readonly IApplicationDbContext _context;

    public SetPayerIdCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        SetPayerIdCommand request,
        CancellationToken cancellationToken)
    {
        var account = await _context.Accounts
            .FirstOrDefaultAsync(x => x.Id == request.AccountId);

        if (account == null)
        {
            throw new NotFoundException(nameof(Account), request.AccountId);
        }

        account.PlayerId = request.PlayerId;

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}