using Application.Common.DTO.Experiences;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Experiences.Queries.GetExperiencesOfStudentWithPaging;

public class GetExperiencesOfStudentWithPagingQuery
    : IRequest<PaginatedList<ExperienceDTO>>
{
    public Guid StudentId { get; init; }
    public bool? IsStudentMustBeVerified { get; init; }

    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string OrderByExpression { get; init; } = string.Empty;
}

public class GetExperiencesOfStudentWithPagingQueryHandler
    : IRequestHandler<GetExperiencesOfStudentWithPagingQuery, PaginatedList<ExperienceDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetExperiencesOfStudentWithPagingQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<ExperienceDTO>> Handle(
        GetExperiencesOfStudentWithPagingQuery request,
        CancellationToken cancellationToken)
    {
        if (!await _context.Students
            .Filter(isVerified: request.IsStudentMustBeVerified)
            .AnyAsync(x => x.Id == request.StudentId))
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        return await _context.Experiences
            .Where(x => x.StudentId == request.StudentId)
            .MapToExperienceDTO()
            .OrderByExpression(request.OrderByExpression)
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}