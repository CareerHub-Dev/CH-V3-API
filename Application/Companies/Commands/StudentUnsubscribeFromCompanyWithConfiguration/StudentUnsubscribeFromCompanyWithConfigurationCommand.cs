using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Commands.StudentUnsubscribeFromCompanyWithConfiguration;

public record StudentUnsubscribeFromCompanyWithConfigurationCommand : IRequest
{
    public Guid StudentId { get; init; }
    public bool? IsStudentVerified { get; init; }
    public Guid CompanytId { get; init; }
    public bool? IsCompanyVerified { get; init; }
}

public class StudentUnsubscribeFromCompanyWithConfigurationCommandHandler : IRequestHandler<StudentUnsubscribeFromCompanyWithConfigurationCommand>
{
    private readonly IApplicationDbContext _context;

    public StudentUnsubscribeFromCompanyWithConfigurationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(StudentUnsubscribeFromCompanyWithConfigurationCommand request, CancellationToken cancellationToken)
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

        var subscribedStudent = company.SubscribedStudents.FirstOrDefault(x => x.Id == request.StudentId);

        if (subscribedStudent == null)
        {
            return Unit.Value;
        }

        company.SubscribedStudents.Remove(subscribedStudent);

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}