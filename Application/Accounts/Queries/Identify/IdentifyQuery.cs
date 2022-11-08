using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.Queries.Identify;

public record IdentifyQuery : IRequest<IdentifyResult?>
{
    public string JwtToken { get; init; } = string.Empty;
}

public class IdentifyQueryHandler : IRequestHandler<IdentifyQuery, IdentifyResult?>
{
    private readonly IApplicationDbContext _context;
    private readonly IJwtService _jwtService;
    private readonly IAccountHelper _accountHelper;

    public IdentifyQueryHandler(IApplicationDbContext context, IJwtService jwtService, IAccountHelper accountHelper)
    {
        _context = context;
        _jwtService = jwtService;
        _accountHelper = accountHelper;
    }

    public async Task<IdentifyResult?> Handle(IdentifyQuery request, CancellationToken cancellationToken)
    {
        var accountId = await _jwtService.ValidateJwtTokenAsync(request.JwtToken);

        if (accountId == null) return null;

        var account = await _context.Accounts
            .AsNoTracking()
            .Include(x => x.Bans.Where(x => x.Expires >= DateTime.UtcNow))
            .FirstOrDefaultAsync(a => a.Id == accountId);

        if (account == null) return null;

        return new IdentifyResult
        {
            Id = account.Id,
            Role = _accountHelper.GetRole(account),
            IsVerified = account.IsVerified,
            IsBanned = account.Bans.Any()
        };
    }
}