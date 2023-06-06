using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobOffers.Queries.IsVerifiedStudentSubscribedToActiveJobOfferWithVerifiedCompany;

public record DidStudentApplyToJobOfferQuery
    : IRequest<bool>
{
    public Guid StudentId { get; init; }
    public Guid JobOfferId { get; init; }
}

public class DidStudentApplyToJobOfferQueryHandler
    : IRequestHandler<DidStudentApplyToJobOfferQuery, bool>
{
    private readonly IApplicationDbContext _context;

    public DidStudentApplyToJobOfferQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(
        DidStudentApplyToJobOfferQuery request,
        CancellationToken cancellationToken)
    {
        var student = await _context.Students
            .Filter(isVerified: true)
            .FirstOrDefaultAsync(x => x.Id == request.StudentId);

        if (student is null)
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        var application = await _context.CVJobOffers
            .Include(x => x.CV)
            .ThenInclude(x => x!.Student)
            .FirstOrDefaultAsync(x => x.CV!.Student!.Id == request.StudentId && x.JobOffer!.Id == request.JobOfferId);

        return application is not null;
    }
}