using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CompanyLinks.Command.DeleteCompanyLinkOfCompany;

public record DeleteCompanyLinkOfCompanyCommand(Guid CompanyLinkId, Guid CompanyId) : IRequest;

public class DeleteCompanyLinkOfCompanyCommandHandler : IRequestHandler<DeleteCompanyLinkOfCompanyCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteCompanyLinkOfCompanyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteCompanyLinkOfCompanyCommand request, CancellationToken cancellationToken)
    {
        if (!await _context.Companies
            .AnyAsync(x => x.Id == request.CompanyId))
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        var companyLink = await _context.CompanyLinks
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.CompanyLinkId && x.CompanyId == request.CompanyId);

        if (companyLink == null)
        {
            throw new NotFoundException(nameof(CompanyLink), request.CompanyLinkId);
        }

        _context.CompanyLinks.Remove(companyLink);

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}