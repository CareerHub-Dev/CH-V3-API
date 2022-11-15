using Application.Common.DTO.Students;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Queries.GetDetailedStudent;

public record GetDetailedStudentQuery
    : IRequest<DetailedStudentDTO>
{
    public Guid StudentId { get; init; }
    public bool? IsStudentMustBeVerified { get; init; }
}

public class GetDetailedStudentQueryHandler
    : IRequestHandler<GetDetailedStudentQuery, DetailedStudentDTO>
{
    private readonly IApplicationDbContext _context;

    public GetDetailedStudentQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DetailedStudentDTO> Handle(
        GetDetailedStudentQuery request,
        CancellationToken cancellationToken)
    {
        var student = await _context.Students
            .AsNoTracking()
            .Where(x => x.Id == request.StudentId)
            .Filter(isVerified: request.IsStudentMustBeVerified)
            .MapToDetailedStudentDTO()
            .FirstOrDefaultAsync();

        if (student == null)
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        return student;
    }
}