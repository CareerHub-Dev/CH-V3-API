using Application.Common.DTO.Experiences;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Experiences.Queries.GetExperienceOfStudent;

public record GetExperienceOfStudentQuery
    : IRequest<ExperienceDTO>
{
    public Guid ExperienceId { get; init; }

    public Guid StudentId { get; init; }
    public bool? IsStudentMustBeVerified { get; init; }
}

public class GetExperienceOfStudentQueryHandler
    : IRequestHandler<GetExperienceOfStudentQuery, ExperienceDTO>
{
    private readonly IApplicationDbContext _context;

    public GetExperienceOfStudentQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ExperienceDTO> Handle(
        GetExperienceOfStudentQuery request, 
        CancellationToken cancellationToken)
    {
        if (!await _context.Students
            .Filter(isVerified: request.IsStudentMustBeVerified)
            .AnyAsync(x => x.Id == request.StudentId))
        {
            throw new NotFoundException(nameof(Company), request.StudentId);
        }

        var experience = await _context.Experiences
            .MapToExperienceDTO()
            .FirstOrDefaultAsync(x => x.Id == request.ExperienceId && x.StudentId == request.StudentId);

        if (experience == null)
        {
            throw new NotFoundException(nameof(CompanyLink), request.ExperienceId);
        }

        return experience;
    }
}