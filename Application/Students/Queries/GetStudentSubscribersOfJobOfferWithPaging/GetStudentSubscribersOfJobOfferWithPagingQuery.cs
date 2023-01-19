using Application.Common.DTO.Students;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Queries.GetStudentSubscribersOfJobOfferWithPaging;

public record GetStudentSubscribersOfJobOfferWithPagingQuery
    : IRequest<PaginatedList<StudentDTO>>
{
    public Guid JobOfferId { get; init; }
    public bool? IsCompanyOfJobOfferMustBeVerified { get; init; }
    public bool? IsJobOfferMustBeActive { get; init; }

    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string SearchTerm { get; init; } = string.Empty;

    public bool? IsStudentSubscriberMustBeVerified { get; init; }
    public Guid? WithoutStudentSubscriberId { get; init; }
    public List<Guid>? StudentGroupIds { get; init; }

    public string OrderByExpression { get; init; } = string.Empty;
}

public class GetStudentSubscribersOfJobOfferWithPagingHandler
    : IRequestHandler<GetStudentSubscribersOfJobOfferWithPagingQuery, PaginatedList<StudentDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetStudentSubscribersOfJobOfferWithPagingHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<StudentDTO>> Handle(
        GetStudentSubscribersOfJobOfferWithPagingQuery request,
        CancellationToken cancellationToken)
    {
        if (!await _context.JobOffers
            .Filter(isActive: request.IsJobOfferMustBeActive, isCompanyVerified: request.IsCompanyOfJobOfferMustBeVerified)
            .AnyAsync(x => x.Id == request.JobOfferId))
        {
            throw new NotFoundException(nameof(JobOffer), request.JobOfferId);
        }

        return await _context.Students
            .Where(x => x.JobOfferSubscriptions.Any(x => x.Id == request.JobOfferId))
            .Filter(
                withoutStudentId: request.WithoutStudentSubscriberId,
                isVerified: request.IsStudentSubscriberMustBeVerified,
                studentGroupIds: request.StudentGroupIds
            )
            .Search(request.SearchTerm)
            .MapToStudentDTO()
            .OrderByExpression(request.OrderByExpression)
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}