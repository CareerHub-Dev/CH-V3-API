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
    private readonly IAccountHelper _accountHelper;

    public ChangeActivationStatusCommandHandler(IApplicationDbContext context, IAccountHelper accountHelper)
    {
        _context = context;
        _accountHelper = accountHelper;
    }

    public async Task<Unit> Handle(ChangeActivationStatusCommand request, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts
            .FirstOrDefaultAsync(x => x.Id == request.AccountId);

        if (account == null)
        {
            throw new NotFoundException(nameof(Account), request.AccountId);
        }

        if(_accountHelper.GetRole(account) == "SuperAdmin")
        {
            throw new ArgumentException("It is forbidden to change the activation status of the super admin.");
        }

        account.ActivationStatus = request.ActivationStatus;

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}
