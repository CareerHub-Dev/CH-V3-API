using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CompanyLinks.Command.CompanyOwnsCompanyLink;

public record CompanyOwnsCompanyLinkCommand : IRequest<bool>
{
    public Guid CompanyId { get; init; }
    public Guid CompanyLinkId { get; init; }
}

public class CompanyOwnsCompanyLinkCommandHandler : IRequestHandler<CompanyOwnsCompanyLinkCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public CompanyOwnsCompanyLinkCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(CompanyOwnsCompanyLinkCommand request, CancellationToken cancellationToken)
    {
        if (!await _context.Companies.AnyAsync(x => x.Id == request.CompanyId))
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        return await _context.CompanyLinks
                .AnyAsync(x => x.CompanyId == request.CompanyId && x.Id == request.CompanyLinkId);
    }
}