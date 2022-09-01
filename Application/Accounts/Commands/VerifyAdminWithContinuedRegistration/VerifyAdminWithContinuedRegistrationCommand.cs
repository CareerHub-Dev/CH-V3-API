using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.Commands.VerifyAdminWithContinuedRegistration;

public record VerifyAdminWithContinuedRegistrationCommand : IRequest
{
    public string Token { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}

public class VerifyAdminWithContinuedRegistrationCommandHandler : IRequestHandler<VerifyAdminWithContinuedRegistrationCommand>
{
    private readonly IApplicationDbContext _context;

    public VerifyAdminWithContinuedRegistrationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(VerifyAdminWithContinuedRegistrationCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Admins
                .SingleOrDefaultAsync(x => x.VerificationToken == request.Token);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Admin), request.Token);
        }

        entity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        entity.VerificationToken = null;
        entity.Verified = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}