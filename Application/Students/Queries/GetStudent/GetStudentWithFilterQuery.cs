using Application.Common.DTO.Students;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Queries.GetStudent;

public record GetStudentWithFilterQuery : IRequest<StudentDTO>
{
    public Guid StudentId { get; init; }
    public bool? IsStudentMustBeVerified { get; init; }
}

public class GetStudentWithFilterQueryHandler
    : IRequestHandler<GetStudentWithFilterQuery, StudentDTO>
{
    private readonly IApplicationDbContext _context;

    public GetStudentWithFilterQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StudentDTO> Handle(
        GetStudentWithFilterQuery request,
        CancellationToken cancellationToken)
    {
        var student = await _context.Students
            .AsNoTracking()
            .Where(x => x.Id == request.StudentId)
            .Filter(
                isVerified: request.IsStudentMustBeVerified
            )
            .MapToStudentDTO()
            .FirstOrDefaultAsync();

        if (student == null)
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        return student;
    }
}