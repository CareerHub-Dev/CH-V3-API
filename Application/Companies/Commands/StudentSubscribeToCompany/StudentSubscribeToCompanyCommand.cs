using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Commands.StudentSubscribeToCompany;

public record StudentSubscribeToCompanyCommand : IRequest
{
    public Guid StudentId { get; init; }
    public bool? IsStudentVerified { get; init; }
    public Guid CompanytId { get; init; }
    public bool? IsCompanyVerified { get; init; }
}

public class StudentSubscribeToCompanyCommandHandler : IRequestHandler<StudentSubscribeToCompanyCommand>
{
    private readonly IApplicationDbContext _context;

    public StudentSubscribeToCompanyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(StudentSubscribeToCompanyCommand request, CancellationToken cancellationToken)
    {
        var student = await _context.Students
            .Filter(isVerified: request.IsStudentVerified)
            .FirstOrDefaultAsync(x => x.Id == request.StudentId);

        if (student == null)
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        var company = await _context.Companies
            .Filter(isVerified: request.IsStudentVerified)
            .Include(x => x.SubscribedStudents)
            .FirstOrDefaultAsync(x => x.Id == request.StudentId);

        if (company == null)
        {
            throw new NotFoundException(nameof(Company), request.CompanytId);
        }

        if (company.SubscribedStudents.Any(x => x.Id == request.StudentId))
        {
            return Unit.Value;
        }

        company.SubscribedStudents.Add(student);

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}