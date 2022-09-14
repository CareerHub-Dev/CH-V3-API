using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Commands.VerifiedStudentSubscribeToVerifiedCompany;

public record VerifiedStudentSubscribeToVerifiedCompanyCommand : IRequest
{
    public Guid StudentId { get; init; }
    public Guid CompanyId { get; init; }
}

public class VerifiedStudentSubscribeToVerifiedCompanyCommandHandler : IRequestHandler<VerifiedStudentSubscribeToVerifiedCompanyCommand>
{
    private readonly IApplicationDbContext _context;

    public VerifiedStudentSubscribeToVerifiedCompanyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(VerifiedStudentSubscribeToVerifiedCompanyCommand request, CancellationToken cancellationToken)
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
            throw new NotFoundException(nameof(Company), request.CompanyId);
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