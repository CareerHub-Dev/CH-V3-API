using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Queries;

public class IsVerifiedStudentSubscribedToVerifiedCompanyQuery : IRequest<bool>
{
    public Guid StudentId { get; init; }
    public Guid CompanyId { get; init; }
}

public class IsVerifiedStudentSubscribedToVerifiedCompanyQueryHandler : IRequestHandler<IsVerifiedStudentSubscribedToVerifiedCompanyQuery, bool>
{
    private readonly IApplicationDbContext _context;

    public IsVerifiedStudentSubscribedToVerifiedCompanyQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(IsVerifiedStudentSubscribedToVerifiedCompanyQuery request, CancellationToken cancellationToken)
    {
        var student = await _context.Students
            .Filter(isVerified: true)
            .FirstOrDefaultAsync(x => x.Id == request.StudentId);

        if (student == null)
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        var company = await _context.Companies
            .Filter(isVerified: true)
            .Include(x => x.SubscribedStudents)
            .FirstOrDefaultAsync(x => x.Id == request.CompanyId);

        if (company == null)
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        if (company.SubscribedStudents.Any(x => x.Id == request.StudentId))
        {
            return true;
        }

        return false;
    }
}