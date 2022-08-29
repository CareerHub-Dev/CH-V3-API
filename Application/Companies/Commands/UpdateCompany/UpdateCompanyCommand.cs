using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Commands.UpdateCompany;

public record UpdateCompanyCommand : IRequest
{
    public Guid CompanyId { get; init; }
    public string CompanyName { get; init; } = string.Empty;
    public string CompanyMotto { get; init; } = string.Empty;
    public string CompanyDescription { get; init; } = string.Empty;
}

public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateCompanyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Companies.FirstOrDefaultAsync(x => x.Id == request.CompanyId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        entity.CompanyName = request.CompanyName;
        entity.CompanyMotto = request.CompanyMotto;
        entity.CompanyDescription = request.CompanyDescription;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}