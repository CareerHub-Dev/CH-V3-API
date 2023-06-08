using Application.Common.DTO.CVs;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CVs.Queries.GetBriefCVWithStatussOfJobOfferWithPaging;

public record GetBriefCVWithStatusAndStudentWithPagingQuery
    : IRequest<PaginatedList<BriefCVWithStatusAndStudentDTO>>
{
    public Guid JobOfferId { get; init; }
    public bool? IsCompanyOfJobOfferMustBeVerified { get; init; }
    public bool? IsJobOfferMustBeActive { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string OrderByExpression { get; init; } = string.Empty;
}

public class GetBriefCVWithStatusAndStudentWithPagingQueryHandler
    : IRequestHandler<GetBriefCVWithStatusAndStudentWithPagingQuery, PaginatedList<BriefCVWithStatusAndStudentDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetBriefCVWithStatusAndStudentWithPagingQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<BriefCVWithStatusAndStudentDTO>> Handle(
        GetBriefCVWithStatusAndStudentWithPagingQuery request,
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
            .Include(x => x.CV)
            .ThenInclude(x => x!.Student)
            .ThenInclude(x => x!.StudentGroup)
            .MapToBriefCVWithStatusAndStudentDTO()
            .OrderByExpression(request.OrderByExpression)
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}