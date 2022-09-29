using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Commands.VerifiedActiveStudentSubscribeToVerifiedActiveCompany;

public record VerifiedActiveStudentSubscribeToVerifiedActiveCompanyCommand : IRequest
{
    public Guid StudentId { get; init; }
    public Guid CompanyId { get; init; }
}

public class VerifiedActiveStudentSubscribeToVerifiedActiveCompanyCommandHandler : IRequestHandler<VerifiedActiveStudentSubscribeToVerifiedActiveCompanyCommand>
{
    private readonly IApplicationDbContext _context;

    public VerifiedActiveStudentSubscribeToVerifiedActiveCompanyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(VerifiedActiveStudentSubscribeToVerifiedActiveCompanyCommand request, CancellationToken cancellationToken)
    {
        var student = await _context.Students
            .Filter(isVerified: true, activationStatus: ActivationStatus.Active)
            .FirstOrDefaultAsync(x => x.Id == request.StudentId);

        if (student == null)
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        var company = await _context.Companies
            .Include(x => x.SubscribedStudents)
            .Filter(isVerified: true, activationStatus: ActivationStatus.Active)
            .FirstOrDefaultAsync(x => x.Id == request.CompanyId);

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