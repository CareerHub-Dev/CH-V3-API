using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Queries;

public class IsStudentSubscribedToCompanyQuery : IRequest<bool>
{
    public Guid StudentId { get; init; }
    public bool? IsStudentVerified { get; init; }
    public Guid CompanyId { get; init; }
    public bool? IsCompanyVerified { get; init; }
}

public class IsStudentSubscribedToCompanyQueryHandler : IRequestHandler<IsStudentSubscribedToCompanyQuery, bool>
{
    private readonly IApplicationDbContext _context;

    public IsStudentSubscribedToCompanyQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(IsStudentSubscribedToCompanyQuery request, CancellationToken cancellationToken)
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
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        if (company.SubscribedStudents.Any(x => x.Id == request.StudentId))
        {
            return true;
        }

        return false;
    }
}