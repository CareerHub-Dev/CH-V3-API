using Application.Common.DTO.JobOfferReviews;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CVs.Queries.GetReviewsWithPagingAsCompany;

public record GetReviewsWithPagingAsCompanyQuery
    : IRequest<PaginatedList<DetailedJobOfferReviewDTO>>
{
    public required Guid CompanyId;
    public required string OrderByExpression { get; init; }
    public required int PageNumber { get; init; }
    public required int PageSize { get; init; }
    public required Review? Status { get; init; }
}

public class GetReviewsWithPagingAsCompanyQueryHandler
    : IRequestHandler<GetReviewsWithPagingAsCompanyQuery, PaginatedList<DetailedJobOfferReviewDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetReviewsWithPagingAsCompanyQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<DetailedJobOfferReviewDTO>> Handle(
        GetReviewsWithPagingAsCompanyQuery request,
        CancellationToken cancellationToken)
    {
        var company = await _context.Companies
            .AsNoTracking()
            .Filter(isVerified: true)
            .FirstOrDefaultAsync(x => x.Id == request.CompanyId, cancellationToken: cancellationToken);
        if (company is null)
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        var reviews = await _context.CVJobOffers
            .AsNoTracking()
            .Include(x => x.JobOffer)
            .ThenInclude(x => x!.Company)
            .Include(x => x.CV)
            .ThenInclude(x => x!.Student)
            .ThenInclude(x => x!.StudentGroup)
            .Where(x => x.JobOffer!.CompanyId == request.CompanyId && x.JobOffer.EndDate.Date > DateTime.UtcNow.Date)
            .Filter(status: request.Status)
            .MapToDetailedJobOfferReviewDTO()
            .OrderByExpression(request.OrderByExpression)
            .ToPagedListAsync(request.PageNumber, request.PageSize, cancellationToken: cancellationToken);
        return reviews;
    }
}