using Application.Common.DTO.CVs;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CVs.Queries.GetBriefCVWithStatussOfJobOfferWithPaging;

public record GetBriefCVWithStatussOfJobOfferWithPagingQuery
    : IRequest<PaginatedList<BriefCVWithStatusDTO>>
{
    public Guid JobOfferId { get; init; }
    public bool? IsCompanyOfJobOfferMustBeVerified { get; init; }
    public bool? IsJobOfferMustBeActive { get; init; }

    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string OrderByExpression { get; init; } = string.Empty;
}

public class GetBriefCVWithStatussOfJobOfferWithPagingQueryHandler
    : IRequestHandler<GetBriefCVWithStatussOfJobOfferWithPagingQuery, PaginatedList<BriefCVWithStatusDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetBriefCVWithStatussOfJobOfferWithPagingQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<BriefCVWithStatusDTO>> Handle(
        GetBriefCVWithStatussOfJobOfferWithPagingQuery request,
        CancellationToken cancellationToken)
    {
        if (!await _context.JobOffers
            .Filter(isActive: request.IsJobOfferMustBeActive, isCompanyVerified: request.IsCompanyOfJobOfferMustBeVerified)
            .AnyAsync(x => x.Id == request.JobOfferId))
        {
            throw new NotFoundException(nameof(JobOffer), request.JobOfferId);
        }

        return await _context.JobOffers
            .Where(x => x.Id == request.JobOfferId)
            .SelectMany(x => x.CVJobOffers)
            .MapToBriefCVWithStatusDTO()
            .OrderByExpression(request.OrderByExpression)
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}