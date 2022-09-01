using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Emails.Commands;

public record SendInviteCompanyEmailCommand(Guid CompanyId) : IRequest;

public class SendInviteCompanyEmailCommandHandler : IRequestHandler<SendInviteCompanyEmailCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IEmailService _emailService;
    private readonly IProcedureService _procedureService;

    public SendInviteCompanyEmailCommandHandler(IApplicationDbContext context, IEmailService emailService, IProcedureService procedureService)
    {
        _context = context;
        _emailService = emailService;
        _procedureService = procedureService;
    }

    public async Task<Unit> Handle(SendInviteCompanyEmailCommand request, CancellationToken cancellationToken)
    {
        var company = await _context.Companies.FirstOrDefaultAsync(x => x.Id == request.CompanyId);

        if (company == null)
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        if (company.IsVerified)
        {
            throw new ArgumentException("Company is verified");
        }

        company.VerificationToken = await _procedureService.GenerateAccountVerificationTokenAsync();
        await _context.SaveChangesAsync();

        await _emailService.SendInviteCompanyEmailAsync(company);

        return Unit.Value;
    }
}
