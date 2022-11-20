using Application.Common.DTO.Accounts;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.Queries.GetBriefAccount;

public record GetBriefAccountQuery(Guid AccountId)
    : IRequest<BriefAccountDTO>;

public class GetBriefAccountQueryHandler
    : IRequestHandler<GetBriefAccountQuery, BriefAccountDTO>
{
    private readonly IApplicationDbContext _context;
    private readonly IAccountHelper _accountHelper;

    public GetBriefAccountQueryHandler(
        IApplicationDbContext context,
        IAccountHelper accountHelper)
    {
        _context = context;
        _accountHelper = accountHelper;
    }

    public async Task<BriefAccountDTO> Handle(
        GetBriefAccountQuery request,
        CancellationToken cancellationToken)
    {
        var account = await _context.Accounts
            .AsNoTracking()
            .Include(x => x.Bans)
            .FirstOrDefaultAsync(x => x.Id == request.AccountId);

        if (account == null)
        {
            throw new NotFoundException(nameof(Account), request.AccountId);
        }

        return new BriefAccountDTO
        {
            Id = account.Id,
            Email = account.Email,
            Verified = account.Verified,
            PasswordReset = account.PasswordReset,
            Role = _accountHelper.GetRole(account),
            IsBanned = _accountHelper.IsBanned(account)
        };
    }
}