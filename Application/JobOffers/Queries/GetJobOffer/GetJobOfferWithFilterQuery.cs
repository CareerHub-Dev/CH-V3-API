using Application.Common.DTO.Companies;
using Application.Common.DTO.JobOffers;
using Application.Common.DTO.JobPositions;
using Application.Common.DTO.Tags;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobOffers.Queries.GetJobOffer;

public record GetJobOfferWithFilterQuery : IRequest<JobOfferDTO>
{
    public Guid JobOfferId { get; set; }
    public bool? IsJobOfferMustBeActive { get; init; }

    public bool? IsCompanyOfJobOfferMustBeVerified { get; init; }
    public ActivationStatus? CompanyOfJobOfferMustHaveActivationStatus { get; init; }
}

public class GetJobOfferWithFilterQueryHandler : IRequestHandler<GetJobOfferWithFilterQuery, JobOfferDTO>
{
    private readonly IApplicationDbContext _context;

    public GetJobOfferWithFilterQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<JobOfferDTO> Handle(GetJobOfferWithFilterQuery request, CancellationToken cancellationToken)
    {
        var jobOffer = await _context.JobOffers
            .AsNoTracking()
            .Filter(
                isActive: request.IsJobOfferMustBeActive,
                isCompanyVerified: request.IsCompanyOfJobOfferMustBeVerified,
                companyActivationStatus: request.CompanyOfJobOfferMustHaveActivationStatus
            )
            .Where(x => x.Id == request.JobOfferId)
            .Select(x => new JobOfferDTO
            {
                Id = x.Id,
                Title = x.Title,
                ImageId = x.ImageId,
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