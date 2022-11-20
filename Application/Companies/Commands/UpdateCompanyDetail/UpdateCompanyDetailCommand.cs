using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Commands.UpdateCompanyDetail;

public record UpdateCompanyDetailCommand
    : IRequest
{
    public Guid CompanyId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Motto { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
}

public class UpdateCompanyDetailCommandHandler
    : IRequestHandler<UpdateCompanyDetailCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateCompanyDetailCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        UpdateCompanyDetailCommand request,
        CancellationToken cancellationToken)
    {
        var company = await _context.Companies
            .FirstOrDefaultAsync(x => x.Id == request.CompanyId);

        if (company == null)
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        company.Name = request.Name;
        company.Motto = request.Motto;
        company.Description = request.Description;

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}