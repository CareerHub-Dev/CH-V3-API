using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Queries.GetAmount;

public record GetAmountStudentSubscribersOfCompanyWithFilterQuery : IRequest<int>
{
    public Guid CompanyId { get; init; }
    public bool? IsCompanyMustBeVerified { get; init; }

    public bool? IsSubscriberMustBeVerified { get; init; }
}

public class GetAmountSubscribersOfCompanyWithFilterQueryHandler
    : IRequestHandler<GetAmountStudentSubscribersOfCompanyWithFilterQuery, int>
{
    private readonly IApplicationDbContext _context;

    public GetAmountSubscribersOfCompanyWithFilterQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(GetAmountStudentSubscribersOfCompanyWithFilterQuery request, CancellationToken cancellationToken)
    {
        if (!await _context.Companies
            .Filter(
                isVerified: request.IsCompanyMustBeVerified
            )
            .AnyAsync(x => x.Id == request.CompanyId))
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        return await _context.Companies
            .Where(x => x.Id == request.CompanyId)
            .SelectMany(x => x.SubscribedStudents)
            .Filter(
                isVerified: request.IsSubscriberMustBeVerified
            )
            .CountAsync();
    }
}