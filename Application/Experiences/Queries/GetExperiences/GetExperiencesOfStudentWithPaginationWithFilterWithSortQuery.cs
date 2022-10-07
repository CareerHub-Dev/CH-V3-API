using Application.Common.DTO.Experiences;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Experiences.Queries.GetExperiences;

public class GetExperiencesOfStudentWithPaginationWithFilterWithSortQuery : IRequest<PaginatedList<ExperienceDTO>>
{
    public Guid StudentId { get; init; }
    public bool? IsStudentMustBeVerified { get; init; }
    public ActivationStatus StudentMustHaveActivationStatus { get; init; }

    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string OrderByExpression { get; init; } = string.Empty;
}

public class GetExperiencesOfStudentWithPaginationWithFilterWithSortQueryHandler : IRequestHandler<GetExperiencesOfStudentWithPaginationWithFilterWithSortQuery, PaginatedList<ExperienceDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetExperiencesOfStudentWithPaginationWithFilterWithSortQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<ExperienceDTO>> Handle(GetExperiencesOfStudentWithPaginationWithFilterWithSortQuery request, CancellationToken cancellationToken)
    {
        if (!await _context.Students
            .Filter(
                isVerified: request.IsStudentMustBeVerified,
                activationStatus: request.StudentMustHaveActivationStatus
            )
            .AnyAsync(x => x.Id == request.StudentId))
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
                EndDate = x.EndDate
            })
            .OrderByExpression(request.OrderByExpression)
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}