using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Commands.VerifiedStudentUnsubscribeFromVerifiedCompany;

public record VerifiedStudentUnsubscribeFromVerifiedCompanyCommand : IRequest
{
    public Guid StudentId { get; init; }
    public Guid CompanytId { get; init; }
}

public class VerifiedStudentUnsubscribeFromVerifiedCompanyCommandHandler : IRequestHandler<VerifiedStudentUnsubscribeFromVerifiedCompanyCommand>
{
    private readonly IApplicationDbContext _context;

    public VerifiedStudentUnsubscribeFromVerifiedCompanyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(VerifiedStudentUnsubscribeFromVerifiedCompanyCommand request, CancellationToken cancellationToken)
    {
        var student = await _context.Students
            .Filter(isVerified: true)
            .FirstOrDefaultAsync(x => x.Id == request.StudentId);

        if (student == null)
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        var company = await _context.Companies
            .Include(x => x.SubscribedStudents)
            .Filter(isVerified: true)
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