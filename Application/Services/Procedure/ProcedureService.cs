using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Application.Services.Procedure;

public class ProcedureService : IProcedureService
{
    private readonly IApplicationDbContext _context;

    public ProcedureService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<string> GenerateAccountResetTokenAsync(CancellationToken cancellationToken = default)
    {
        var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));

        var tokenIsUnique = !await _context.Accounts.AnyAsync(x => x.ResetToken == token, cancellationToken);
        if (!tokenIsUnique)
            return await GenerateAccountResetTokenAsync(cancellationToken);

        return token;
    }

    public async Task<string> GenerateAccountVerificationTokenAsync(CancellationToken cancellationToken = default)
    {
        var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));

        var tokenIsUnique = !await _context.Accounts.AnyAsync(x => x.VerificationToken == token, cancellationToken);
        if (!tokenIsUnique)
            return await GenerateAccountVerificationTokenAsync(cancellationToken);

        return token;
    }
}
