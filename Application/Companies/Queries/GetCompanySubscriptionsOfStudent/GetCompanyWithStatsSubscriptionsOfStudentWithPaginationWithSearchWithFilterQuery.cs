using Application.Common.DTO.Companies;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Application.Companies.Queries.Models;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Queries.GetCompanySubscriptionsOfStudent;

public record GetCompanyWithStatsSubscriptionsOfStudentWithPaginationWithSearchWithFilterQuery
    : IRequest<PaginatedList<CompanyWithStatsDTO>>
{
    public Guid StudentOwnerId { get; init; }
    public bool? IsStudentOwnerMustBeVerified { get; init; }

    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string? SearchTerm { get; init; }

    public bool? IsCompanyMustBeVerified { get; init; }
    public Guid? WithoutCompanyId { get; init; }
    public ActivationStatus? ActivationStatus { get; init; }

    public StatsFilter StatsFilter { get; init; } = new StatsFilter();
}

public class GetCompanyWithStatsSubscriptionsOfStudentWithPaginationWithSearchWithFilterQueryHandler
    : IRequestHandler<GetCompanyWithStatsSubscriptionsOfStudentWithPaginationWithSearchWithFilterQuery, PaginatedList<CompanyWithStatsDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetCompanyWithStatsSubscriptionsOfStudentWithPaginationWithSearchWithFilterQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<CompanyWithStatsDTO>> Handle(
        GetCompanyWithStatsSubscriptionsOfStudentWithPaginationWithSearchWithFilterQuery request, 
        CancellationToken cancellationToken)
    {
        if (!await _context.Students
            .Filter(isVerified: request.IsStudentOwnerMustBeVerified)
            .AnyAsync(x => x.Id == request.StudentOwnerId))
        {
            throw new NotFoundException(nameof(Student), request.StudentOwnerId);
        }

        return await _context.Companies
            .AsNoTracking()
            .Filter(
                withoutCompanyId: request.WithoutCompanyId,
                isVerified: request.IsCompanyMustBeVerified,
                activationStatus: request.ActivationStatus
            )
            .Search(request.SearchTerm ?? "")
            .OrderBy(x => x.Name)
            .Where(x => x.SubscribedStudents.Any(x => x.Id == request.StudentOwnerId))
            .Select(x => new CompanyWithStatsDTO
            {
                Id = x.Id,
                Email = x.Email,
                Name = x.Name,
                LogoId = x.LogoId,
                BannerId = x.BannerId,
                Motto = x.Motto,
                Description = x.Description,
                AmountJobOffers = x.JobOffers.Filter(request.StatsFilter.IsJobOfferMustBeActive, null).Count(),
                AmountSubscribers = x.SubscribedStudents.Filter(null, request.StatsFilter.IsSubscriberMustBeVerified, null).Count(),
                Verified = x.Verified,
                PasswordReset = x.PasswordReset,
                ActivationStatus = x.ActivationStatus
            })
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}
