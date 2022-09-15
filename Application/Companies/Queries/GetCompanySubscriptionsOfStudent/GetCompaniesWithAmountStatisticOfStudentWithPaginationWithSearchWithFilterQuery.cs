using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Application.Companies.Queries.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Queries.GetCompanySubscriptionsOfStudent;

public record GetCompaniesWithAmountStatisticOfStudentWithPaginationWithSearchWithFilterQuery
    : IRequest<PaginatedList<CompanyWithAmountStatisticDTO>>
{
    public Guid StudentOwnerId { get; init; }
    public bool? IsStudentOwnerMustBeVerified { get; init; }

    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string? SearchTerm { get; init; }

    public bool? IsCompanyMustBeVerified { get; init; }
    public Guid? WithoutCompanyId { get; init; }

    public bool? IsJobOfferMustBeActive { get; init; }
    public bool? IsSubscriberMustBeVerified { get; init; }
}

public class GetCompaniesWithAmountStatisticOfStudentWithPaginationWithSearchWithFilterQueryHandler
    : IRequestHandler<GetCompaniesWithAmountStatisticOfStudentWithPaginationWithSearchWithFilterQuery, PaginatedList<CompanyWithAmountStatisticDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetCompaniesWithAmountStatisticOfStudentWithPaginationWithSearchWithFilterQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<CompanyWithAmountStatisticDTO>> Handle(GetCompaniesWithAmountStatisticOfStudentWithPaginationWithSearchWithFilterQuery request, CancellationToken cancellationToken)
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
                isVerified: request.IsCompanyMustBeVerified
            )
            .Search(request.SearchTerm ?? "")
            .OrderBy(x => x.Name)
            .Where(x => x.SubscribedStudents.Any(x => x.Id == request.StudentOwnerId))
            .Select(x => new CompanyWithAmountStatisticDTO
            {
                Id = x.Id,
                Email = x.Email,
                Name = x.Name,
                LogoId = x.LogoId,
                BannerId = x.BannerId,
                Motto = x.Motto,
                Description = x.Description,
                AmountStatistic = new AmountStatistic
                {
                    AmountJobOffers = x.JobOffers.Filter(request.IsJobOfferMustBeActive, null).Count(),
                    AmountSubscribers = x.SubscribedStudents.Filter(null, request.IsSubscriberMustBeVerified, null).Count()
                },
                Verified = x.Verified,
                PasswordReset = x.PasswordReset,
            })
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}
