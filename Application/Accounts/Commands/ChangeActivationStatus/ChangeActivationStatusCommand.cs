using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.Commands.ChangeActivationStatus;

public record ChangeActivationStatusCommand : IRequest
{
    public Guid AccountId { get; init; }
    public ActivationStatus ActivationStatus { get; init; }
}

public class ChangeActivationStatusCommandHandler : IRequestHandler<ChangeActivationStatusCommand>
{
    private readonly IApplicationDbContext _context;

    public ChangeActivationStatusCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(ChangeActivationStatusCommand request, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts
            .FirstOrDefaultAsync(x => x.Id == request.AccountId);

        if (account == null)
        {
            throw new NotFoundException(nameof(Account), request.AccountId);
        }

        account.ActivationStatus = request.ActivationStatus;

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}
