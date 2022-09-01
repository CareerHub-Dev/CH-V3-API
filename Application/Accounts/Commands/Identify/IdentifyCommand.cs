using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.Commands.Identify;

public record IdentifyCommand : IRequest<IdentifyResponse?>
{
    public string JwtToken { get; init; } = string.Empty;
}

public class IdentifyCommandHandler : IRequestHandler<IdentifyCommand, IdentifyResponse?>
{
    private readonly IApplicationDbContext _context;
    private readonly IJwtService _jwtService;

    public IdentifyCommandHandler(IApplicationDbContext context, IJwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    public async Task<IdentifyResponse?> Handle(IdentifyCommand request, CancellationToken cancellationToken)
    {
        var accountId = await _jwtService.ValidateJwtTokenAsync(request.JwtToken);

        if (accountId == null) return null;

        var account = await _context.Accounts.AsNoTracking().FirstOrDefaultAsync(a => a.Id == accountId);

        if (account == null) return null;

        return new IdentifyResponse { Id = account.Id, Role = account.GetType().Name, IsVerified = account.IsVerified };
    }
}