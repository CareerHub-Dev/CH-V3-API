using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Queries.GetAmountCompanySubscriptionsOfStudent;

public record GetAmountCompanySubscriptionsOfStudentQuery
    : IRequest<int>
{
    public Guid StudentId { get; init; }
    public bool? IsStudentMustBeVerified { get; init; }

    public bool? IsCompanyMustBeVerified { get; init; }
}

public class GetAmountCompanySubscriptionsOfStudentQueryHandler
    : IRequestHandler<GetAmountCompanySubscriptionsOfStudentQuery, int>
{
    private readonly IApplicationDbContext _context;

    public GetAmountCompanySubscriptionsOfStudentQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(
        GetAmountCompanySubscriptionsOfStudentQuery request,
        CancellationToken cancellationToken)
    {
        if (!await _context.Students
            .Filter(isVerified: request.IsStudentMustBeVerified)
            .AnyAsync(x => x.Id == request.StudentId))
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        return await _context.Students
            .Where(x => x.Id == request.StudentId)
            .SelectMany(x => x.CompanySubscriptions)
            .Filter(isVerified: request.IsCompanyMustBeVerified)
            .CountAsync();
    }
}