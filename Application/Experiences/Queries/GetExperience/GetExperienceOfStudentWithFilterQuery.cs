using Application.Common.DTO.Experiences;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Experiences.Queries.GetExperience;

public record GetExperienceOfStudentWithFilterQuery : IRequest<ExperienceDTO>
{
    public Guid ExperienceId { get; init; }

    public Guid StudentId { get; init; }
    public bool? IsStudentMustBeVerified { get; init; }
    public ActivationStatus StudentMustHaveActivationStatus { get; init; }
}

public class GetExperienceOfStudentWithFilterQueryHandler : IRequestHandler<GetExperienceOfStudentWithFilterQuery, ExperienceDTO>
{
    private readonly IApplicationDbContext _context;

    public GetExperienceOfStudentWithFilterQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ExperienceDTO> Handle(GetExperienceOfStudentWithFilterQuery request, CancellationToken cancellationToken)
    {
        if (!await _context.Students
            .Filter(
                isVerified: request.IsStudentMustBeVerified,
                activationStatus: request.StudentMustHaveActivationStatus
            )
            .AnyAsync(x => x.Id == request.StudentId))
        {
            throw new NotFoundException(nameof(Company), request.StudentId);
        }

        var companyLink = await _context.Experiences
            .AsNoTracking()
            .Where(x => x.Id == request.ExperienceId && x.StudentId == request.StudentId)
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
            .FirstOrDefaultAsync();

        if (companyLink == null)
        {
            throw new NotFoundException(nameof(CompanyLink), request.ExperienceId);
        }

        return companyLink;
    }
}