using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Commands.VerifiedActiveStudentUnsubscribeFromVerifiedActiveCompany;

public record VerifiedActiveStudentUnsubscribeFromVerifiedActiveCompanyCommand : IRequest
{
    public Guid StudentId { get; init; }
    public Guid CompanyId { get; init; }
}

public class VerifiedActiveStudentUnsubscribeFromVerifiedActiveCompanyCommandHandler : IRequestHandler<VerifiedActiveStudentUnsubscribeFromVerifiedActiveCompanyCommand>
{
    private readonly IApplicationDbContext _context;

    public VerifiedActiveStudentUnsubscribeFromVerifiedActiveCompanyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(VerifiedActiveStudentUnsubscribeFromVerifiedActiveCompanyCommand request, CancellationToken cancellationToken)
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
            .FirstOrDefaultAsync(x => x.Id == request.CompanyId);

        if (company == null)
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
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