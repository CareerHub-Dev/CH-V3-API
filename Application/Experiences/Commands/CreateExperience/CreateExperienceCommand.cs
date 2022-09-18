using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Experiences.Commands.CreateExperience;

public record CreateExperienceCommand : IRequest<Guid>
{
    public string Title { get; init; } = string.Empty;
    public string CompanyName { get; init; } = string.Empty;
    public JobType JobType { get; init; }
    public WorkFormat WorkFormat { get; init; }
    public ExperienceLevel ExperienceLevel { get; init; }
    public string JobLocation { get; init; } = string.Empty;
    public DateTime StartDate { get; init; }
    public DateTime? EndDate { get; init; }

    public Guid StudentId { get; init; }
}

public class CreateExperienceCommandHandler : IRequestHandler<CreateExperienceCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateExperienceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateExperienceCommand request, CancellationToken cancellationToken)
    {
        if (!await _context.Students.AnyAsync(x => x.Id == request.StudentId))
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        var experience = new Experience
        {
            Title = request.Title,
            CompanyName = request.CompanyName,
            JobType = request.JobType,
            WorkFormat = request.WorkFormat,
            ExperienceLevel = request.ExperienceLevel,
            JobLocation = request.JobLocation,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            StudentId = request.StudentId
        };

        await _context.Experiences.AddAsync(experience);

        await _context.SaveChangesAsync();

        return experience.Id;
    }
}