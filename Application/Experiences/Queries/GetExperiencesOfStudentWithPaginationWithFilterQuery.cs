using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Experiences.Queries;

public class GetExperiencesOfStudentWithPaginationWithFilterQuery : IRequest<PaginatedList<ExperienceDTO>>
{
    public Guid StudentId { get; init; }
    public bool? IsStudentMustBeVerified { get; init; }

    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetExperiencesOfStudentWithPaginationWithFilterQueryHandler : IRequestHandler<GetExperiencesOfStudentWithPaginationWithFilterQuery, PaginatedList<ExperienceDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetExperiencesOfStudentWithPaginationWithFilterQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<ExperienceDTO>> Handle(GetExperiencesOfStudentWithPaginationWithFilterQuery request, CancellationToken cancellationToken)
    {
        if (!await _context.Students.Filter(isVerified: request.IsStudentMustBeVerified).AnyAsync(x => x.Id == request.StudentId))
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        return await _context.Experiences
            .AsNoTracking()
            .Where(x => x.StudentId == request.StudentId)
            .Select(x => new ExperienceDTO
            {
                Id = x.Id,
                Title = x.Title,
                CompanyName = x.CompanyName,
                JobType = x.JobType,
                WorkFormat = x.WorkFormat,
                ExperienceLevel = x.ExperienceLevel,
                JobLocation = x.JobLocation,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                StudentId = x.StudentId
            })
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}