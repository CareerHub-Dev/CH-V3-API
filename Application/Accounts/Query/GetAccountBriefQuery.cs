using Application.Accounts.Query.Models;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.Query;

public record GetAccountBriefQuery(Guid AccountId) : IRequest<AccountBriefDTO>;

public class GetStudentLogQueryHandler : IRequestHandler<GetAccountBriefQuery, AccountBriefDTO>
{
    private readonly IApplicationDbContext _context;

    public GetStudentLogQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AccountBriefDTO> Handle(GetAccountBriefQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Accounts
            .AsNoTracking()
            .Where(x => x.Id == request.AccountId)
            .FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Account), request.AccountId);
        }

        return new AccountBriefDTO { 
            Id = entity.Id, 
            Email = entity.Email, 
            Verified = entity.Verified, 
            PasswordReset = entity.PasswordReset,
            Role = entity.GetType().Name 
        };
    }
}