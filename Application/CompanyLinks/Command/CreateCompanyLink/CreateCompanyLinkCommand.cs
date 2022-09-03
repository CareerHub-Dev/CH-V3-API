using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CompanyLinks.Command.CreateCompanyLink;

public record CreateCompanyLinkCommand : IRequest<Guid>
{
    public string Name { get; init; } = string.Empty;
    public string Uri { get; init; } = string.Empty;

    public Guid CompanyId { get; init; }
}

public class CreateCompanyLinkCommandHandler : IRequestHandler<CreateCompanyLinkCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateCompanyLinkCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateCompanyLinkCommand request, CancellationToken cancellationToken)
    {
        var company = await _context.Companies
            .Include(x => x.Links)
            .FirstOrDefaultAsync(x => x.Id == request.CompanyId);

        if(company == null)
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        if (company.Links.Count >= 5)
        {
            throw new ArgumentOutOfRangeException(nameof(request), "Maximum of 5 links per company.");
        }

        var companyLink = new CompanyLink
        {
            Title = request.Name,
            Uri = request.Uri,
        };

        company.Links.Add(companyLink);

        await _context.SaveChangesAsync();

        return companyLink.Id;
    }
}