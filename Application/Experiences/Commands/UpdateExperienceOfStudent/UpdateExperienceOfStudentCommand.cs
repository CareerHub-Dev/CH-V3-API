using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Experiences.Commands.UpdateExperienceOfStudent;

public record UpdateExperienceOfStudentCommand
    : IRequest
{
    public Guid ExperienceId { get; init; }
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

public class UpdateExperienceOfStudentCommandHandler
    : IRequestHandler<UpdateExperienceOfStudentCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateExperienceOfStudentCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        UpdateExperienceOfStudentCommand request, 
        CancellationToken cancellationToken)
    {
        if (!await _context.Students
            .AnyAsync(x => x.Id == request.StudentId))
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        var experience = await _context.Experiences
            .Where(x => x.Id == request.ExperienceId && x.StudentId == request.StudentId)
            .FirstOrDefaultAsync();

        if (experience == null)
        {
            throw new NotFoundException(nameof(Experience), request.ExperienceId);
        }

        experience.Title = request.Title;
        experience.CompanyName = request.CompanyName;
        experience.JobType = request.JobType;
        experience.WorkFormat = request.WorkFormat;
        experience.ExperienceLevel = request.ExperienceLevel;
        experience.JobLocation = request.JobLocation;
        experience.StartDate = request.StartDate;
        experience.EndDate = request.EndDate;

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}