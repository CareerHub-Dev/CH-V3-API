using Application.Common.DTO.Students;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Queries.GetStudent;

public record GetStudentQuery : IRequest<StudentDTO>
{
    public Guid StudentId { get; init; }
    public bool? IsStudentMustBeVerified { get; init; }
}

public class GetStudentQueryHandler
    : IRequestHandler<GetStudentQuery, StudentDTO>
{
    private readonly IApplicationDbContext _context;

    public GetStudentQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StudentDTO> Handle(
        GetStudentQuery request,
        CancellationToken cancellationToken)
    {
        var student = await _context.Students
            .AsNoTracking()
            .Where(x => x.Id == request.StudentId)
            .Filter(isVerified: request.IsStudentMustBeVerified)
            .MapToStudentDTO()
            .FirstOrDefaultAsync();

        if (student == null)
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        return student;
    }
}