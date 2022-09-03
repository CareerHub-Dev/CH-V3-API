using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CompanyLinks.Command.UpdateCompanyLink;

public record UpdateCompanyLinkCommand : IRequest
{
    public Guid CompanyLinkId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Uri { get; init; } = string.Empty;
}

public class UpdateCompanyLinkCommandHandler : IRequestHandler<UpdateCompanyLinkCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateCompanyLinkCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateCompanyLinkCommand request, CancellationToken cancellationToken)
    {
        var companyLink = await _context.CompanyLinks
            .FirstOrDefaultAsync(x => x.Id == request.CompanyLinkId);

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