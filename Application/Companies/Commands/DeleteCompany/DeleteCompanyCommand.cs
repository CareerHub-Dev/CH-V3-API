using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Commands.DeleteCompany;

public record DeleteCompanyCommand(Guid ComapnyId)
    : IRequest;

public class DeleteCompanyCommandHandler
    : IRequestHandler<DeleteCompanyCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteCompanyCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        DeleteCompanyCommand request,
        CancellationToken cancellationToken)
    {
        var company = await _context.Companies
            .FirstOrDefaultAsync(x => x.Id == request.ComapnyId);

        if (company == null)
        {
            throw new NotFoundException(nameof(Company), request.ComapnyId);
        }

        _context.Companies.Remove(company);

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}