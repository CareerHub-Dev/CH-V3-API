using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Queries;

public record GetAmountCompanySubscriptionsOfStudentWithFilterQuery : IRequest<int>
{
    public Guid StudentId { get; init; }
    public bool? IsVerified { get; init; }

    public bool? IsCompanyVerified { get; init; }
}

public class GetAmountCompanySubscriptionsOfStudentWithFilterQueryHandler
    : IRequestHandler<GetAmountCompanySubscriptionsOfStudentWithFilterQuery, int>
{
    private readonly IApplicationDbContext _context;

    public GetAmountCompanySubscriptionsOfStudentWithFilterQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(GetAmountCompanySubscriptionsOfStudentWithFilterQuery request, CancellationToken cancellationToken)
    {
        if (!await _context.Students
            .Filter(isVerified: request.IsVerified)
            .AnyAsync(x => x.Id == request.StudentId))
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        return await _context.Students
            .Where(x => x.Id == request.StudentId)
            .SelectMany(x => x.CompanySubscriptions)!
            .Filter(isVerified: request.IsCompanyVerified)
            .CountAsync();
    }
}