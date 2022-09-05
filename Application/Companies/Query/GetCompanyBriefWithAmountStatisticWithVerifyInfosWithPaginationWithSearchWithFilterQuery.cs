﻿using Application.Common.Entensions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Application.Companies.Query.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Query;

public record GetCompanyBriefWithAmountStatisticWithVerifyInfosWithPaginationWithSearchWithFilterQuery 
    : IRequest<PaginatedList<CompanyBriefWithAmountStatisticWithVerificationInfoDTO>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string? SearchTerm { get; init; }

    public bool? IsVerified { get; init; }
    public Guid? WithoutCompanyId { get; init; }

    public bool? IsJobOfferActive { get; init; }
    public bool? IsSubscriberVerified { get; init; }
}

public class GetCompanyBriefWithAmountStatisticWithVerifyInfosWithPaginationWithSearchWithFilterQueryHandler
    : IRequestHandler<GetCompanyBriefWithAmountStatisticWithVerifyInfosWithPaginationWithSearchWithFilterQuery, PaginatedList<CompanyBriefWithAmountStatisticWithVerificationInfoDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetCompanyBriefWithAmountStatisticWithVerifyInfosWithPaginationWithSearchWithFilterQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<CompanyBriefWithAmountStatisticWithVerificationInfoDTO>> Handle(GetCompanyBriefWithAmountStatisticWithVerifyInfosWithPaginationWithSearchWithFilterQuery request, CancellationToken cancellationToken)
    {
        return await _context.Companies
            .AsNoTracking()
            .Filter(request.WithoutCompanyId, request.IsVerified)
            .Search(request.SearchTerm ?? "")
            .OrderBy(x => x.Name)
            .Select(x => new CompanyBriefWithAmountStatisticWithVerificationInfoDTO
            {
                Id = x.Id,
                Email = x.Email,
                Name = x.Name,
                LogoId = x.LogoId,
                BannerId = x.BannerId,
                AmountStatistic = new AmountStatistic
                {
                    AmountJobOffers = x.JobOffers.Filter(request.IsSubscriberVerified).Count(),
                    AmountSubscribers = x.SubscribedStudents.Filter(null, request.IsSubscriberVerified).Count()
                },
                Verified = x.Verified,
                PasswordReset = x.PasswordReset,
            })
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}
