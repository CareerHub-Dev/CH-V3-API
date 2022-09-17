using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CompanyLinks.Command.UpdateCompanyLinkOfCompany;

public record UpdateCompanyLinkOfCompanyCommand : IRequest
{
    public Guid CompanyLinkId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Uri { get; init; } = string.Empty;

    public Guid CompanyId { get; init; }
}

public class UpdateCompanyLinkCommandHandler : IRequestHandler<UpdateCompanyLinkOfCompanyCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateCompanyLinkCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateCompanyLinkOfCompanyCommand request, CancellationToken cancellationToken)
    {
        if (!await _context.Companies
            .AnyAsync(x => x.Id == request.CompanyId))
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        var companyLink = await _context.CompanyLinks
            .FirstOrDefaultAsync(x => x.Id == request.CompanyLinkId && x.CompanyId == request.CompanyId);

        if (companyLink == null)
        {
            throw new NotFoundException(nameof(CompanyLink), request.CompanyLinkId);
        }

        companyLink.Title = request.Name;
        companyLink.Uri = request.Uri;

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}