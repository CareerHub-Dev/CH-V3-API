using Application.Common.DTO.Companies;
using Application.Common.DTO.JobOffers;
using Application.Common.DTO.JobPositions;
using Application.Common.DTO.Tags;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Application.Companies.Queries.GetCompanies;
using Application.JobOffers.Queries.Models;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobOffers.Queries.GetJobOffers;

public record GetFollowedDetiledJobOffersWithStatsForFollowerStudentWithPaginationWithSearchWithFilterWithSortQuery
    : IRequest<PaginatedList<FollowedDetiledJobOfferWithStatsDTO>>
{
    public Guid FollowerStudentId { get; init; }
    public bool? IsFollowerStudentMustBeVerified { get; init; }
    public ActivationStatus? FollowerStudentMustHaveActivationStatus { get; init; }

    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string SearchTerm { get; init; } = string.Empty;

    public bool? IsJobOfferMustBeActive { get; init; }
    public JobType? MustHaveJobType { get; set; }
    public WorkFormat? MustHaveWorkFormat { get; set; }
    public ExperienceLevel? MustHaveExperienceLevel { get; set; }
    public Guid? MustHaveJobPositionId { get; set; }
    public List<Guid>? MustHaveTagIds { get; set; } = new List<Guid>();
    public bool? IsCompanyOfJobOfferMustBeVerified { get; init; }
    public ActivationStatus? CompanyOfJobOfferMustHaveActivationStatus { get; init; }

    public StatsFilter StatsFilter { get; init; } = new StatsFilter();

    public string OrderByExpression { get; init; } = string.Empty;
}

public class GetFollowedDetiledJobOffersWithStatsForFollowerStudentWithPaginationWithSearchWithFilterWithSortQueryHandler
    : IRequestHandler<GetFollowedDetiledJobOffersWithStatsForFollowerStudentWithPaginationWithSearchWithFilterWithSortQuery, PaginatedList<FollowedDetiledJobOfferWithStatsDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetFollowedDetiledJobOffersWithStatsForFollowerStudentWithPaginationWithSearchWithFilterWithSortQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<FollowedDetiledJobOfferWithStatsDTO>> Handle(
        GetFollowedDetiledJobOffersWithStatsForFollowerStudentWithPaginationWithSearchWithFilterWithSortQuery request,
        CancellationToken cancellationToken)
    {
        if (!await _context.Students
            .Filter(
                isVerified: request.IsFollowerStudentMustBeVerified,
                activationStatus: request.FollowerStudentMustHaveActivationStatus
            )
            .AnyAsync(x => x.Id == request.FollowerStudentId))
        {
            throw new NotFoundException(nameof(Student), request.FollowerStudentId);
        }

        return await _context.JobOffers
            .AsNoTracking()
            .Filter(
                isActive: request.IsJobOfferMustBeActive,
                isCompanyVerified: request.IsCompanyOfJobOfferMustBeVerified,
                companyActivationStatus: request.CompanyOfJobOfferMustHaveActivationStatus,
                jobType: request.MustHaveJobType,
                workFormat: request.MustHaveWorkFormat,
                experienceLevel: request.MustHaveExperienceLevel,
                jobPositionId: request.MustHaveJobPositionId,
                tagIds: request.MustHaveTagIds
            )
            .Search(request.SearchTerm)
            .Select(x => new FollowedDetiledJobOfferWithStatsDTO
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
                IsFollowed = x.SubscribedStudents.Any(x => x.Id == request.FollowerStudentId),
            })
            .OrderByExpression(request.OrderByExpression)
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}