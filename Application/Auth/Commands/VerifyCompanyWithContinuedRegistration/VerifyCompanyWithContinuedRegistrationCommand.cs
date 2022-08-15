using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Auth.Commands.VerifyCompanyWithContinuedRegistration;

public record VerifyCompanyWithContinuedRegistrationCommand : IRequest
{
    public string Token { get; set; } = string.Empty;

    public string CompanyName { get; set; } = string.Empty;
    public string CompanyMotto { get; set; } = string.Empty;
    public string CompanyDescription { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class VerifyCompanyWithContinuedRegistrationCommandHandler : IRequestHandler<VerifyCompanyWithContinuedRegistrationCommand>
{
    private readonly IApplicationDbContext _context;

    public VerifyCompanyWithContinuedRegistrationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(VerifyCompanyWithContinuedRegistrationCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Companies
                .SingleOrDefaultAsync(x => x.VerificationToken == request.Token, cancellationToken);

        if (entity == null)
        {
            throw new ArgumentException("Verification failed. Token does not exist");
        }

        entity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        entity.VerificationToken = null;
        entity.Verified = DateTime.UtcNow;
        entity.CompanyName = request.CompanyName;
        entity.CompanyMotto = request.CompanyMotto;
        entity.CompanyDescription = request.CompanyDescription;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
