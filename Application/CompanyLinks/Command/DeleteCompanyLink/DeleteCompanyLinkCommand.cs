using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CompanyLinks.Command.DeleteCompanyLink;

public record DeleteCompanyLinkCommand(Guid CompanyLinkId) : IRequest;

public class DeleteCompanyLinkCommandHandler : IRequestHandler<DeleteCompanyLinkCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteCompanyLinkCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteCompanyLinkCommand request, CancellationToken cancellationToken)
    {
        var companyLink = await _context.CompanyLinks
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.CompanyLinkId);

        if (companyLink == null)
        {
            throw new NotFoundException(nameof(CompanyLink), request.CompanyLinkId);
        }

        _context.CompanyLinks.Remove(companyLink);

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}