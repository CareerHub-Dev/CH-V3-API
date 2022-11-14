using Application.Common.DTO.JobOffers;
using Application.Common.DTO.JobPositions;
using Application.Common.DTO.Tags;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobOffers.Queries.GetJobOffer;

public record GetJobOfferOfCompanyWithFilterQuery : IRequest<JobOfferDTO>
{
    public Guid JobOfferId { get; set; }
    public bool? IsJobOfferMustBeActive { get; init; }

    public Guid CompanyId { get; init; }
    public bool? IsCompanyOfJobOfferMustBeVerified { get; init; }
}

public class GetJobOfferOfCompanyWithFilterQueryHandler : IRequestHandler<GetJobOfferOfCompanyWithFilterQuery, JobOfferDTO>
{
    private readonly IApplicationDbContext _context;

    public GetJobOfferOfCompanyWithFilterQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<JobOfferDTO> Handle(GetJobOfferOfCompanyWithFilterQuery request, CancellationToken cancellationToken)
    {
        if (!await _context.Companies
            .Filter(
                isVerified: request.IsCompanyOfJobOfferMustBeVerified
            )
            .AnyAsync(x => x.Id == request.CompanyId))
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        var jobOffer = await _context.JobOffers
            .AsNoTracking()
            .Filter(
                isActive: request.IsJobOfferMustBeActive
            )
            .Where(x => x.Id == request.JobOfferId && x.CompanyId == request.CompanyId)
            .Select(x => new JobOfferDTO
            {
                Id = x.Id,
                Title = x.Title,
                Image = x.Image,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                JobType = x.JobType,
                WorkFormat = x.WorkFormat,
                ExperienceLevel = x.ExperienceLevel,
                JobPosition = new BriefJobPositionDTO { Id = x.JobPosition!.Id, Name = x.JobPosition.Name },
                Company = new BriefCompanyDTO { Id = x.Company!.Id, Name = x.Company.Name },
                Tags = x.Tags.Select(y => new TagDTO { Id = y.Id, Name = y.Name }).ToList(),
                Overview = x.Overview,
                Requirements = x.Requirements,
                Responsibilities = x.Responsibilities,
                Preferences = x.Preferences
            })
            .FirstOrDefaultAsync();

        if (jobOffer == null)
        {
            throw new NotFoundException(nameof(JobOffer), request.JobOfferId);
        }

        return jobOffer;
    }
}