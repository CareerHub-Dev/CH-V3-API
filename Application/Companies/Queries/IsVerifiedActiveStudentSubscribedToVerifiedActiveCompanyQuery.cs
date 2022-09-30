using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Queries;

public class IsVerifiedActiveStudentSubscribedToVerifiedActiveCompanyQuery : IRequest<bool>
{
    public Guid StudentId { get; init; }
    public Guid CompanyId { get; init; }
}

public class IsVerifiedActiveStudentSubscribedToVerifiedActiveCompanyQueryHandler : IRequestHandler<IsVerifiedActiveStudentSubscribedToVerifiedActiveCompanyQuery, bool>
{
    private readonly IApplicationDbContext _context;

    public IsVerifiedActiveStudentSubscribedToVerifiedActiveCompanyQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(IsVerifiedActiveStudentSubscribedToVerifiedActiveCompanyQuery request, CancellationToken cancellationToken)
    {
        var student = await _context.Students
            .Filter(isVerified: true, activationStatus: ActivationStatus.Active)
            .FirstOrDefaultAsync(x => x.Id == request.StudentId);

        if (student == null)
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        var company = await _context.Companies
            .Filter(isVerified: true)
            .Include(x => x.SubscribedStudents.Where(x => x.Id == request.StudentId))
            .FirstOrDefaultAsync(x => x.Id == request.CompanyId);

        if (company == null)
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        if (company.SubscribedStudents.Any())
        {
            return true;
        }

        return false;
    }
}