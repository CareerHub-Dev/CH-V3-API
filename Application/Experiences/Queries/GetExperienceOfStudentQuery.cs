﻿using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Experiences.Queries;

public record GetExperienceOfStudentQuery : IRequest<ExperienceDTO>
{
    public Guid ExperienceId { get; init; }

    public Guid StudentId { get; init; }
    public bool? IsStudentMustBeVerified { get; init; }
}

public class GetCompanyLinkOfCompanyQueryHandler : IRequestHandler<GetExperienceOfStudentQuery, ExperienceDTO>
{
    private readonly IApplicationDbContext _context;

    public GetCompanyLinkOfCompanyQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ExperienceDTO> Handle(GetExperienceOfStudentQuery request, CancellationToken cancellationToken)
    {
        if (!await _context.Students.Filter(isVerified: request.IsStudentMustBeVerified).AnyAsync(x => x.Id == request.StudentId))
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
                EndDate = x.EndDate,
                StudentId = x.StudentId
            })
            .FirstOrDefaultAsync();

        if (companyLink == null)
        {
            throw new NotFoundException(nameof(CompanyLink), request.ExperienceId);
        }

        return companyLink;
    }
}