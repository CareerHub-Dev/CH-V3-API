﻿using Application.Common.DTO.CVProjectLinks;
using Application.Common.DTO.Educations;
using Application.Common.DTO.ForeignLanguages;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.CVs.Commands.CreateCV;

public record CreateCVCommand 
    : IRequest<Guid>
{
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

public class CreateCVCommandHandler
    : IRequestHandler<CreateCVCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IImagesService _imagesService;

    public CreateCVCommandHandler(
        IApplicationDbContext context,
        IImagesService imagesService)
    {
        _context = context;
        _imagesService = imagesService;
    }

    public async Task<Guid> Handle(
        CreateCVCommand request, 
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

        var cv = new CV
        {
            Title = request.Title,
            JobPositionId = request.JobPositionId,
            TemplateLanguage = request.TemplateLanguage,
            LastName = request.LastName,
            FirstName = request.FirstName,
            Goals = request.Goals,
            SkillsAndTechnologies = request.SkillsAndTechnologies,
            ExperienceHighlights = request.ExperienceHighlights,
            StudentId = request.StudentId,
            ForeignLanguages = request.ForeignLanguages.Select(x => new ForeignLanguage
            {
                Name = x.Name,
                LanguageLevel = x.LanguageLevel,
            }).ToList(),
            ProjectLinks = request.ProjectLinks.Select(x => new CVProjectLink
            {
                Title = x.Title,
                Url = x.Url,
            }).ToList(),
            Educations = request.Educations.Select(x => new Education
            {
                University = x.University,
                City = x.City,
                Country = x.Country,
                Specialty = x.Specialty,
                Degree = x.Degree,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
            }).ToList(),
            Created = DateTime.UtcNow
        };

        if (request.Photo != null)
        {
            cv.Photo = await _imagesService.SaveImageAsync(request.Photo);
        }

        await _context.CVs.AddAsync(cv);

        await _context.SaveChangesAsync();

        return cv.Id;
    }
}