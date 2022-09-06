using Application.Common.Helpers;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.Query.Identify;

public record IdentifyQuery : IRequest<IdentifyResult?>
{
    public string JwtToken { get; init; } = string.Empty;
}

public class IdentifyQueryHandler : IRequestHandler<IdentifyQuery, IdentifyResult?>
{
    private readonly IApplicationDbContext _context;
    private readonly IJwtService _jwtService;

    public IdentifyQueryHandler(IApplicationDbContext context, IJwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    public async Task<IdentifyResult?> Handle(IdentifyQuery request, CancellationToken cancellationToken)
    {
        var accountId = await _jwtService.ValidateJwtTokenAsync(request.JwtToken);

        if (accountId == null) return null;

        var account = await _context.Accounts.AsNoTracking().FirstOrDefaultAsync(a => a.Id == accountId);

        if (account == null) return null;

        return new IdentifyResult { Id = account.Id, Role = AccountHelper.GetRole(account), IsVerified = account.IsVerified };
    }
}