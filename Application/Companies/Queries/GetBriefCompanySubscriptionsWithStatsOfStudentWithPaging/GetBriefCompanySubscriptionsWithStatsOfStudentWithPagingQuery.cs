using Application.Common.DTO.Companies;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Application.Companies.Queries.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Queries.GetBriefCompanySubscriptionsWithStatsOfStudentWithPaging;

public record GetBriefCompanySubscriptionsWithStatsOfStudentWithPagingQuery
    : IRequest<PaginatedList<BriefCompanyWithStatsDTO>>
{
    public Guid StudentId { get; init; }
    public bool? IsStudentMustBeVerified { get; init; }

    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string SearchTerm { get; init; } = string.Empty;

    public bool? IsCompanyMustBeVerified { get; init; }
    public Guid? WithoutCompanyId { get; init; }

    public StatsFilter StatsFilter { get; init; } = new StatsFilter();

    public string OrderByExpression { get; init; } = string.Empty;
}

public class GetBriefCompanySubscriptionsWithStatsOfStudentWithPagingQueryHandler
    : IRequestHandler<GetBriefCompanySubscriptionsWithStatsOfStudentWithPagingQuery, PaginatedList<BriefCompanyWithStatsDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetBriefCompanySubscriptionsWithStatsOfStudentWithPagingQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<BriefCompanyWithStatsDTO>> Handle(
        GetBriefCompanySubscriptionsWithStatsOfStudentWithPagingQuery request,
        CancellationToken cancellationToken)
    {
        if (!await _context.Students
            .Filter(isVerified: request.IsStudentMustBeVerified)
            .AnyAsync(x => x.Id == request.StudentId))
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        return await _context.Companies
            .Filter(
                withoutCompanyId: request.WithoutCompanyId,
                isVerified: request.IsCompanyMustBeVerified
            )
            .Search(request.SearchTerm)
            .Where(x => x.SubscribedStudents.Any(x => x.Id == request.StudentId))
            .MapToBriefCompanyWithStatsDTO(
                isJobOfferMustBeActive: request.StatsFilter.IsJobOfferMustBeActive,
                isSubscriberMustBeVerified: request.StatsFilter.IsSubscriberMustBeVerified
            )
            .OrderByExpression(request.OrderByExpression)
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}
