using Application.Common.DTO.JobOffers;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobOffers.Queries.GetJobOfferOfCompany;

public record GetJobOfferOfCompanyQuery
    : IRequest<JobOfferDTO>
{
    public Guid JobOfferId { get; set; }
    public bool? IsJobOfferMustBeActive { get; init; }
    public Guid CompanyId { get; init; }
    public bool? IsCompanyOfJobOfferMustBeVerified { get; init; }
}

public class GetJobOfferOfCompanyQueryHandler
    : IRequestHandler<GetJobOfferOfCompanyQuery, JobOfferDTO>
{
    private readonly IApplicationDbContext _context;

    public GetJobOfferOfCompanyQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<JobOfferDTO> Handle(
        GetJobOfferOfCompanyQuery request,
        CancellationToken cancellationToken)
    {
        if (!await _context.Companies
            .Filter(isVerified: request.IsCompanyOfJobOfferMustBeVerified)
            .AnyAsync(x => x.Id == request.CompanyId))
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        var jobOffer = await _context.JobOffers
            .Filter(isActive: request.IsJobOfferMustBeActive)
            .MapToJobOfferDTO()
            .FirstOrDefaultAsync(x => x.Id == request.JobOfferId && x.Company.Id == request.CompanyId);

        if (jobOffer == null)
        {
            throw new NotFoundException(nameof(JobOffer), request.JobOfferId);
        }

        return jobOffer;
    }
}