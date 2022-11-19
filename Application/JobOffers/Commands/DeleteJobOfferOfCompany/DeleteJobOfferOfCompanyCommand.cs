using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobOffers.Commands.DeleteJobOfferOfCompany;

public record DeleteJobOfferOfCompanyCommand(Guid JobOfferId, Guid CompanyId) 
    : IRequest;

public class DeleteJobOfferOfCompanyCommandHandler 
    : IRequestHandler<DeleteJobOfferOfCompanyCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteJobOfferOfCompanyCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        DeleteJobOfferOfCompanyCommand request, 
        CancellationToken cancellationToken)
    {
        if (!await _context.Companies
            .AnyAsync(x => x.Id == request.CompanyId))
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        var jobOffer = await _context.JobOffers
            .Where(x => x.Id == request.JobOfferId && x.CompanyId == request.CompanyId)
            .FirstOrDefaultAsync();

        if (jobOffer == null)
        {
            throw new NotFoundException(nameof(JobOffer), request.JobOfferId);
        }

        _context.JobOffers.Remove(jobOffer);

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}