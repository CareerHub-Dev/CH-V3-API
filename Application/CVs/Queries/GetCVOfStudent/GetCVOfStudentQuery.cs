using Application.Common.DTO.CVs;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CVs.Queries.GetCVOfStudent;

public record GetCVOfStudentQuery
    : IRequest<CVDTO>
{
    public Guid CVId { get; init; }
    public Guid StudentId { get; init; }
    public bool? IsStudentMustBeVerified { get; init; }
}

public class GetCVOfStudentQueryHandler
    : IRequestHandler<GetCVOfStudentQuery, CVDTO>
{
    private readonly IApplicationDbContext _context;

    public GetCVOfStudentQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CVDTO> Handle(
        GetCVOfStudentQuery request,
        CancellationToken cancellationToken)
    {
        if (!await _context.Students
            .Filter(isVerified: request.IsStudentMustBeVerified)
            .AnyAsync(x => x.Id == request.StudentId))
        {
            throw new NotFoundException(nameof(Company), request.StudentId);
        }

        var cv = await _context.CVs
            .MapToCVDTO()
            .FirstOrDefaultAsync(x => x.Id == request.CVId && x.StudentId == request.StudentId);

        if (cv == null)
        {
            throw new NotFoundException(nameof(CV), request.CVId);
        }

        return cv;
    }
}