using Application.Common.DTO.Experiences;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Experiences.Queries.GetExperience;

public record GetExperienceWithFilterQuery : IRequest<ExperienceDTO>
{
    public Guid ExperienceId { get; init; }

    public bool? IsStudentMustBeVerified { get; init; }
    public ActivationStatus StudentMustHaveActivationStatus { get; init; }
}

public class GetExperienceWithFilterQueryHandler : IRequestHandler<GetExperienceWithFilterQuery, ExperienceDTO>
{
    private readonly IApplicationDbContext _context;

    public GetExperienceWithFilterQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ExperienceDTO> Handle(GetExperienceWithFilterQuery request, CancellationToken cancellationToken)
    {
        var experience = await _context.Experiences
            .AsNoTracking()
            .Where(x => x.Id == request.ExperienceId)
            .Filter(
                isStudentVerified: request.IsStudentMustBeVerified,
                studentMustHaveActivationStatus: request.StudentMustHaveActivationStatus
            )
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
            .FirstOrDefaultAsync();

        if (experience == null)
        {
            throw new NotFoundException(nameof(Experience), request.ExperienceId);
        }

        return experience;
    }
}