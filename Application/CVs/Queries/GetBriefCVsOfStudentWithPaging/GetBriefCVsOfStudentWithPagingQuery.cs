using Application.Common.DTO.CVs;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CVs.Queries.GetBriefCVsOfStudentWithPaging;

public class GetBriefCVsOfStudentWithPagingQuery
    : IRequest<PaginatedList<BriefCVDTO>>
{
    public Guid StudentId { get; init; }
    public bool? IsStudentMustBeVerified { get; init; }

    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string OrderByExpression { get; init; } = string.Empty;
}

public class GetBriefCVsOfStudentWithPagingQueryHandler
    : IRequestHandler<GetBriefCVsOfStudentWithPagingQuery, PaginatedList<BriefCVDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetBriefCVsOfStudentWithPagingQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<BriefCVDTO>> Handle(
        GetBriefCVsOfStudentWithPagingQuery request,
        CancellationToken cancellationToken)
    {
        if (!await _context.Students
            .Filter(isVerified: request.IsStudentMustBeVerified)
            .AnyAsync(x => x.Id == request.StudentId))
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        return await _context.CVs
            .Where(x => x.StudentId == request.StudentId)
            .MapToBriefCVDTO()
            .OrderByExpression(request.OrderByExpression)
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}