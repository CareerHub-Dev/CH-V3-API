using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.Commands.DeleteAccount;

public record DeleteAccountCommand(Guid AccountId) : IRequest;

public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteAccountCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.AccountId);

        if (account == null)
        {
            throw new NotFoundException(nameof(Account), request.AccountId);
        }

        _context.Accounts.Remove(account);

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}