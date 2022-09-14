﻿using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Application.Companies.Queries.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Queries.GetCompanies;

public record GetFollowedCompaniesDetailedWithAmountStatisticForFollowerStudentWithPaginationWithSearchWithFilterQuery
    : IRequest<PaginatedList<FollowedCompanyDetailedWithAmountStatisticDTO>>
{
    public Guid FollowerStudentId { get; init; }
    public bool? IsFollowerStudentMustBeVerified { get; init; }

    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string? SearchTerm { get; init; }

    public bool? IsCompanyMustBeVerified { get; init; }
    public Guid? WithoutCompanyId { get; init; }

    public bool? IsJobOfferMustBeActive { get; init; }
    public bool? IsSubscriberMustBeVerified { get; init; }
}

public class GetFollowedCompaniesDetailedWithAmountStatisticForFollowerStudentWithPaginationWithSearchWithFilterQueryHandler
    : IRequestHandler<GetFollowedCompaniesDetailedWithAmountStatisticForFollowerStudentWithPaginationWithSearchWithFilterQuery, PaginatedList<FollowedCompanyDetailedWithAmountStatisticDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetFollowedCompaniesDetailedWithAmountStatisticForFollowerStudentWithPaginationWithSearchWithFilterQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<FollowedCompanyDetailedWithAmountStatisticDTO>> Handle(GetFollowedCompaniesDetailedWithAmountStatisticForFollowerStudentWithPaginationWithSearchWithFilterQuery request, CancellationToken cancellationToken)
    {
        if (!await _context.Students
            .Filter(isVerified: request.IsFollowerStudentMustBeVerified)
            .AnyAsync(x => x.Id == request.FollowerStudentId))
        {
            throw new NotFoundException(nameof(Student), request.FollowerStudentId);
        }

        return await _context.Companies
            .AsNoTracking()
            .Filter(
                withoutCompanyId: request.WithoutCompanyId,
                isVerified: request.IsCompanyMustBeVerified
            )
            .Search(request.SearchTerm ?? "")
            .OrderBy(x => x.Name)
            .Select(x => new FollowedCompanyDetailedWithAmountStatisticDTO
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
                IsFollowed = x.SubscribedStudents.Any(x => x.Id == request.FollowerStudentId),
            })
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}