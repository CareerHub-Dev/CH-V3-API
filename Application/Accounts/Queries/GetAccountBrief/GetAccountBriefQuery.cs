﻿using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.Queries.GetAccountBrief;

public record GetAccountBriefQuery(Guid AccountId) : IRequest<AccountBriefDTO>;

public class GetStudentLogQueryHandler : IRequestHandler<GetAccountBriefQuery, AccountBriefDTO>
{
    private readonly IApplicationDbContext _context;
    private readonly IAccountHelper _accountHelper;
    public GetStudentLogQueryHandler(IApplicationDbContext context, IAccountHelper accountHelper)
    {
        _context = context;
        _accountHelper = accountHelper;
    }

    public async Task<AccountBriefDTO> Handle(GetAccountBriefQuery request, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts
            .AsNoTracking()
            .Where(x => x.Id == request.AccountId)
            .FirstOrDefaultAsync();

        if (account == null)
        {
            throw new NotFoundException(nameof(Account), request.AccountId);
        }

        return new AccountBriefDTO
        {
            Id = account.Id,
            Email = account.Email,
            Verified = account.Verified,
            PasswordReset = account.PasswordReset,
            Role = _accountHelper.GetRole(account)
        };
    }
}