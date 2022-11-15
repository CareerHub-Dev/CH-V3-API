using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Queries.GetAmountJobOfferSubscriptionsOfStudent;

public class GetAmountJobOfferSubscriptionsOfStudentQuery
    : IRequest<int>
{
    public Guid StudentId { get; init; }
    public bool? IsStudentMustBeVerified { get; init; }

    public bool? IsJobOfferMustBeActive { get; init; }
    public bool? IsCompanyOfJobOfferMustBeVerified { get; init; }
}

public class GetAmountJobOfferSubscriptionsOfStudentQueryHandler
    : IRequestHandler<GetAmountJobOfferSubscriptionsOfStudentQuery, int>
{
    private readonly IApplicationDbContext _context;

    public GetAmountJobOfferSubscriptionsOfStudentQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(
        GetAmountJobOfferSubscriptionsOfStudentQuery request,
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
            .SelectMany(x => x.JobOfferSubscriptions)
            .Filter(
                isActive: request.IsJobOfferMustBeActive,
                isCompanyVerified: request.IsCompanyOfJobOfferMustBeVerified
            )
            .CountAsync();

    }
}