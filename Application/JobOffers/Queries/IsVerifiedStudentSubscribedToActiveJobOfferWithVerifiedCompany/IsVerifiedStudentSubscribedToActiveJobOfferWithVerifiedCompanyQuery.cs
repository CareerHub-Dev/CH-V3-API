using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobOffers.Queries.IsVerifiedStudentSubscribedToActiveJobOfferWithVerifiedCompany;

public record IsVerifiedStudentSubscribedToActiveJobOfferWithVerifiedCompanyQuery
    : IRequest<bool>
{
    public Guid StudentId { get; init; }
    public Guid JobOfferId { get; init; }
}

public class IsVerifiedStudentSubscribedToActiveJobOfferWithVerifiedCompanyQueryHandler
    : IRequestHandler<IsVerifiedStudentSubscribedToActiveJobOfferWithVerifiedCompanyQuery, bool>
{
    private readonly IApplicationDbContext _context;

    public IsVerifiedStudentSubscribedToActiveJobOfferWithVerifiedCompanyQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(
        IsVerifiedStudentSubscribedToActiveJobOfferWithVerifiedCompanyQuery request,
        CancellationToken cancellationToken)
    {
        var student = await _context.Students
            .Filter(isVerified: true)
            .FirstOrDefaultAsync(x => x.Id == request.StudentId);

        if (student == null)
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        var joboffer = await _context.JobOffers
             .Include(x => x.SubscribedStudents.Where(x => x.Id == request.StudentId))
             .Filter(
                 isActive: true,
                 isCompanyVerified: true
             )
             .FirstOrDefaultAsync(x => x.Id == request.JobOfferId);

        if (joboffer == null)
        {
            throw new NotFoundException(nameof(JobOffer), request.JobOfferId);
        }

        return joboffer.SubscribedStudents.Any();
    }
}