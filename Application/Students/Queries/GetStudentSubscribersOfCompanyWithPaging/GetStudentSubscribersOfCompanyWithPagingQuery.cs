using Application.Common.DTO.Students;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Queries.GetStudentSubscribersOfCompanyWithPaging;

public record GetStudentSubscribersOfCompanyWithPagingQuery
    : IRequest<PaginatedList<StudentDTO>>
{
    public Guid CompanyId { get; init; }
    public bool? IsCompanyMustBeVerified { get; init; }

    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string SearchTerm { get; init; } = string.Empty;

    public bool? IsStudentSubscriberMustBeVerified { get; init; }
    public Guid? WithoutStudentSubscriberId { get; init; }
    public List<Guid>? StudentGroupIds { get; init; }

    public string OrderByExpression { get; init; } = string.Empty;
}

public class GetStudentSubscribersOfCompanyWithPagingQueryQueryHandler
    : IRequestHandler<GetStudentSubscribersOfCompanyWithPagingQuery, PaginatedList<StudentDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetStudentSubscribersOfCompanyWithPagingQueryQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<StudentDTO>> Handle(
        GetStudentSubscribersOfCompanyWithPagingQuery request,
        CancellationToken cancellationToken)
    {
        if (!await _context.Companies
            .Filter(isVerified: request.IsCompanyMustBeVerified)
            .AnyAsync(x => x.Id == request.CompanyId))
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        return await _context.Students
            .Where(x => x.CompanySubscriptions.Any(x => x.Id == request.CompanyId))
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