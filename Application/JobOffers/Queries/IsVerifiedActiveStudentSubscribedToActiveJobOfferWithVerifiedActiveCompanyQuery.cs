using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Companies.Queries;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobOffers.Queries;

public record IsVerifiedActiveStudentSubscribedToActiveJobOfferWithVerifiedActiveCompanyQuery : IRequest<bool>
{
    public Guid StudentId { get; init; }
    public Guid JobOfferId { get; init; }
}

public class IsVerifiedActiveStudentSubscribedToActiveJobOfferWithVerifiedActiveCompanyQueryHandler
    : IRequestHandler<IsVerifiedActiveStudentSubscribedToActiveJobOfferWithVerifiedActiveCompanyQuery, bool>
{
    private readonly IApplicationDbContext _context;

    public IsVerifiedActiveStudentSubscribedToActiveJobOfferWithVerifiedActiveCompanyQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(
        IsVerifiedActiveStudentSubscribedToActiveJobOfferWithVerifiedActiveCompanyQuery request, 
        CancellationToken cancellationToken)
    {
        var student = await _context.Students
            .Filter(
                isVerified: true,
                activationStatus: ActivationStatus.Active
            )
            .FirstOrDefaultAsync(x => x.Id == request.StudentId);

        if (student == null)
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        var joboffer = await _context.JobOffers
             .Include(x => x.SubscribedStudents.Where(x => x.Id == request.StudentId))
             .Filter(
                 isActive: true,
                 isCompanyVerified: true,
                 companyActivationStatus: ActivationStatus.Active
             )
             .FirstOrDefaultAsync(x => x.Id == request.JobOfferId);

        if (joboffer == null)
        {
            throw new NotFoundException(nameof(JobOffer), request.JobOfferId);
        }

        return joboffer.SubscribedStudents.Any();
    }
}