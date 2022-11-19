using Application.Common.DTO.Experiences;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Experiences.Queries.GetExperience;

public record GetExperienceQuery
    : IRequest<ExperienceDTO>
{
    public Guid ExperienceId { get; init; }

    public bool? IsStudentMustBeVerified { get; init; }
}

public class GetExperienceQueryHandler
    : IRequestHandler<GetExperienceQuery, ExperienceDTO>
{
    private readonly IApplicationDbContext _context;

    public GetExperienceQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ExperienceDTO> Handle(
        GetExperienceQuery request, 
        CancellationToken cancellationToken)
    {
        var experience = await _context.Experiences
            .Filter(isStudentVerified: request.IsStudentMustBeVerified)
            .MapToExperienceDTO()
            .FirstOrDefaultAsync(x => x.Id == request.ExperienceId);

        if (experience == null)
        {
            throw new NotFoundException(nameof(Experience), request.ExperienceId);
        }

        return experience;
    }
}