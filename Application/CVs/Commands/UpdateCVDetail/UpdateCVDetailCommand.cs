using Application.Common.DTO.CVProjectLinks;
using Application.Common.DTO.Educations;
using Application.Common.DTO.ForeignLanguages;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.JobOffers.Commands.UpdateJobOfferDetail;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.CVs.Commands.UpdateCVDetail;

public record UpdateCVDetailCommand 
    : IRequest
{
    public Guid CVId { get; init; }
    public string Title { get; init; } = string.Empty;

    public Guid JobPositionId { get; init; }

    public TemplateLanguage TemplateLanguage { get; init; }
    public string LastName { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public IFormFile? Photo { get; init; }
    public string Goals { get; init; } = string.Empty;
    public string SkillsAndTechnologies { get; init; } = string.Empty;
    public string ExperienceHighlights { get; init; } = string.Empty;

    public Guid StudentId { get; init; }

    public List<ForeignLanguageDTO> ForeignLanguages { get; init; } = new List<ForeignLanguageDTO>();
    public List<CVProjectLinkDTO> ProjectLinks { get; init; } = new List<CVProjectLinkDTO>();
    public List<EducationDTO> Educations { get; init; } = new List<EducationDTO>();
}

public class UpdateCVDetailCommandHandler
    : IRequestHandler<UpdateCVDetailCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateCVDetailCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        UpdateCVDetailCommand request,
        CancellationToken cancellationToken)
    {
        if (!await _context.JobPositions
            .AnyAsync(x => x.Id == request.JobPositionId))
        {
            throw new NotFoundException(nameof(JobPosition), request.JobPositionId);
        }

        var cv = await _context.CVs
            .FirstOrDefaultAsync(x => x.Id == request.CVId);

        if (cv == null)
        {
            throw new NotFoundException(nameof(CV), request.CVId);
        }

        cv.Title = request.Title;
        cv.JobPositionId = request.JobPositionId;
        cv.TemplateLanguage = request.TemplateLanguage;
        cv.LastName = request.LastName;
        cv.FirstName = request.FirstName;
        cv.JobPositionId = request.JobPositionId;
        cv.Goals = request.Goals;
        cv.SkillsAndTechnologies = request.SkillsAndTechnologies;
        cv.ExperienceHighlights = request.ExperienceHighlights;
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
        cv.Modified = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}