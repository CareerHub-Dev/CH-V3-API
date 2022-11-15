using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Queries.GetAmountCVsOfStudent;

public class GetAmountCVsOfStudentQuery
    : IRequest<int>
{
    public Guid StudentId { get; init; }
    public bool? IsStudentMustBeVerified { get; init; }
}

public class GetAmountCVsOfStudentQueryHandler
    : IRequestHandler<GetAmountCVsOfStudentQuery, int>
{
    private readonly IApplicationDbContext _context;

    public GetAmountCVsOfStudentQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(
        GetAmountCVsOfStudentQuery request,
        CancellationToken cancellationToken)
    {
        if (!await _context.Students
            .Filter(isVerified: request.IsStudentMustBeVerified)
            .AnyAsync(x => x.Id == request.StudentId))
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        return await _context.Students
            .Where(x => x.Id == request.StudentId)
            .SelectMany(x => x.CVs)
            .CountAsync();
    }
}