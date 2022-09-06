using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Query;

public record GetCompanyAmountSubscribersWithFilterQuery : IRequest<int>
{
    public Guid CompanyId { get; init; }
    public bool? IsVerified { get; init; }

    public bool? IsSubscriberVerified { get; init; }
}

public class GetCompanyAmountSubscribersWithFilterQueryHandler
    : IRequestHandler<GetCompanyAmountSubscribersWithFilterQuery, int>
{
    private readonly IApplicationDbContext _context;

    public GetCompanyAmountSubscribersWithFilterQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(GetCompanyAmountSubscribersWithFilterQuery request, CancellationToken cancellationToken)
    {
        if(!await _context.Companies
            .Filter(IsVerified: request.IsVerified)
            .AnyAsync(x => x.Id == request.CompanyId))
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        return await _context.Companies
            .Where(x => x.Id == request.CompanyId)
            .SelectMany(x => x.SubscribedStudents)
            .Filter(IsVerified: request.IsSubscriberVerified)
            .CountAsync();
    }
}