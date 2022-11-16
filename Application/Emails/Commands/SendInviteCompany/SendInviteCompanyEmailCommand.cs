using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Emails.Commands.SendInviteCompany;

public record SendInviteCompanyEmailCommand(Guid CompanyId)
    : IRequest;

public class SendInviteCompanyEmailCommandHandler
    : IRequestHandler<SendInviteCompanyEmailCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IEmailService _emailService;
    private readonly IAccountHelper _accountHelper;

    public SendInviteCompanyEmailCommandHandler(
        IApplicationDbContext context,
        IEmailService emailService,
        IAccountHelper accountHelper)
    {
        _context = context;
        _emailService = emailService;
        _accountHelper = accountHelper;
    }

    public async Task<Unit> Handle(
        SendInviteCompanyEmailCommand request, 
        CancellationToken cancellationToken)
    {
        var company = await _context.Companies
            .Where(x => x.Id == request.CompanyId)
            .FirstOrDefaultAsync();

        if (company == null)
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        if (company.IsVerified)
        {
            throw new ArgumentException("Company is verified");
        }

        company.VerificationToken = await _accountHelper.GenerateUniqueVerificationTokenAsync();
        await _context.SaveChangesAsync();

        await _emailService.SendInviteCompanyEmailAsync(company);

        return Unit.Value;
    }
}
