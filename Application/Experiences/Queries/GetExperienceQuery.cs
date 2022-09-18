using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Experiences.Queries;

public record GetExperienceQuery(Guid ExperienceId) : IRequest<ExperienceDTO>;

public class GetExperienceQueryHandler : IRequestHandler<GetExperienceQuery, ExperienceDTO>
{
    private readonly IApplicationDbContext _context;

    public GetExperienceQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ExperienceDTO> Handle(GetExperienceQuery request, CancellationToken cancellationToken)
    {
        var experience = await _context.Experiences
            .AsNoTracking()
            .Where(x => x.Id == request.ExperienceId)
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