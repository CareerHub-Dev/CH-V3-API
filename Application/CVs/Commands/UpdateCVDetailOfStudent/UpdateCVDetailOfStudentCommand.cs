using Application.Common.DTO.CVProjectLinks;
using Application.Common.DTO.Educations;
using Application.Common.DTO.Experiences;
using Application.Common.DTO.ForeignLanguages;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.CVs.Commands.UpdateCVDetailOfStudent;

public record UpdateCVDetailOfStudentCommand
    : IRequest
{
    public required Guid CVId { get; init; }
    public required ExperienceLevel ExperienceLevel { get; init; }
    public required string Title { get; init; } = string.Empty;
    public required Guid JobPositionId { get; init; }
    public required TemplateLanguage TemplateLanguage { get; init; }
    public required string LastName { get; init; } = string.Empty;
    public required string FirstName { get; init; } = string.Empty;
    public required string Goals { get; init; } = string.Empty;
    public required List<string> HardSkills { get; set; } = new List<string>();
    public required List<string> SoftSkills { get; set; } = new List<string>();
    public required List<ForeignLanguageDTO> ForeignLanguages { get; init; } = new List<ForeignLanguageDTO>();
    public required List<CVProjectLinkDTO> ProjectLinks { get; init; } = new List<CVProjectLinkDTO>();
    public required List<EducationDTO> Educations { get; init; } = new List<EducationDTO>();
    public required List<CVExperienceDTO> Experiences { get; init; } = new List<CVExperienceDTO>();
    public required Guid StudentId { get; init; }
}

public class UpdateCVDetailOfStudentCommandHandler
    : IRequestHandler<UpdateCVDetailOfStudentCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateCVDetailOfStudentCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        UpdateCVDetailOfStudentCommand request,
        CancellationToken cancellationToken)
    {
        if (!await _context.Students
            .AnyAsync(x => x.Id == request.StudentId))
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        if (!await _context.JobPositions
            .AnyAsync(x => x.Id == request.JobPositionId))
        {
            throw new NotFoundException(nameof(JobPosition), request.JobPositionId);
        }

        var cv = await _context.CVs
            .FirstOrDefaultAsync(x => x.Id == request.CVId && x.StudentId == request.StudentId);

        if (cv == null)
        {
            throw new NotFoundException(nameof(CV), request.CVId);
        }

        cv.ExperienceLevel = request.ExperienceLevel;
        cv.Title = request.Title;
        cv.JobPositionId = request.JobPositionId;
        cv.TemplateLanguage = request.TemplateLanguage;
        cv.LastName = request.LastName;
        cv.FirstName = request.FirstName;
        cv.JobPositionId = request.JobPositionId;
        cv.Goals = request.Goals;
        cv.HardSkills = request.HardSkills;
        cv.SoftSkills = request.SoftSkills;
        cv.ForeignLanguages = request.ForeignLanguages.Select(x => new ForeignLanguage
        {
            Name = x.Name,
            LanguageLevel = x.LanguageLevel,
        }).ToList();
        cv.ProjectLinks = request.ProjectLinks.Select(x => new CVProjectLink
        {
            Title = x.Title,
            Url = x.Url,
        }).ToList();
        cv.Educations = request.Educations.Select(x => new Education
        {
            University = x.University,
            City = x.City,
            Country = x.Country,
            Specialty = x.Specialty,
            Degree = x.Degree,
            StartDate = x.StartDate,
            EndDate = x.EndDate,
        }).ToList();
        cv.Experiences = request.Experiences.Select(x => new CVExperience
        {
            Title = x.Title,
            CompanyName = x.CompanyName,
            JobType = x.JobType,
            WorkFormat = x.WorkFormat,
            ExperienceLevel = x.ExperienceLevel,
            JobLocation = x.JobLocation,
            StartDate = x.StartDate,
            EndDate = x.EndDate
        }).ToList();
        cv.Modified = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}