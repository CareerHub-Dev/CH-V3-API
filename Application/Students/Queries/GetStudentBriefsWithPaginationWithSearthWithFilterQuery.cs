using Application.Common.Entensions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Application.Students.Queries.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Queries;

public record GetStudentBriefsWithPaginationWithSearthWithFilterQuery : IRequest<PaginatedList<StudentBriefDTO>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string? SearchTerm { get; init; }

    public bool? IsVerified { get; init; }
    public Guid? WithoutStudentId { get; init; }
}

public class GetStudentBriefsWithPaginationWithSearthWithFilterQueryHandler : IRequestHandler<GetStudentBriefsWithPaginationWithSearthWithFilterQuery, PaginatedList<StudentBriefDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetStudentBriefsWithPaginationWithSearthWithFilterQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<StudentBriefDTO>> Handle(GetStudentBriefsWithPaginationWithSearthWithFilterQuery request, CancellationToken cancellationToken)
    {
        return await _context.Students
            .AsNoTracking()
            .Filter(request.WithoutStudentId, request.IsVerified)
            .Search(request.SearchTerm ?? "")
            .Select(x => new StudentBriefDTO
            {
                Id = x.Id,
                Email = x.Email,
                FirstName = x.FirstName,
                LastName = x.LastName,
                PhotoId = x.PhotoId,
            })
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}