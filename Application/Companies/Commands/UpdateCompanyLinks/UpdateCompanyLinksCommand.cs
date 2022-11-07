using Application.Common.DTO.CompanyLinks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Commands.UpdateCompanyLinks;

public record UpdateCompanyLinksCommand : IRequest
{
    public Guid CompanyId { get; init; }
    public List<CompanyLinkDTO> Links { get; init; } = new List<CompanyLinkDTO>();
}

public class UpdateCompanyLinksCommandHandler : IRequestHandler<UpdateCompanyLinksCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateCompanyLinksCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateCompanyLinksCommand request, CancellationToken cancellationToken)
    {
        var company = await _context.Companies.FirstOrDefaultAsync(x => x.Id == request.CompanyId);

        if (company == null)
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        company.Links = request.Links
            .Select(x => new CompanyLink { Title = x.Title, Uri = x.Uri })
            .ToList();

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}