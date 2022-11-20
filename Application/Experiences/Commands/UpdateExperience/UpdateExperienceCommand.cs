using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Experiences.Commands.UpdateExperience;

public record UpdateExperienceCommand
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
}

public class UpdateExperienceCommandHandler
    : IRequestHandler<UpdateExperienceCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateExperienceCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        UpdateExperienceCommand request,
        CancellationToken cancellationToken)
    {
        var experience = await _context.Experiences
            .FirstOrDefaultAsync(x => x.Id == request.ExperienceId);

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