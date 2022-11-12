using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobOffers.Commands.VerifiedStudentUnubscribeFromActiveJobOffer;

public record VerifiedActiveStudentUnsubscribeFromActiveJobOfferWithVerifiedActiveCompanyCommand : IRequest
{
    public Guid StudentId { get; init; }
    public Guid JobOfferId { get; init; }
}

public class VerifiedActiveStudentUnsubscribeFromActiveJobOfferWithVerifiedActiveCompanyCommandHandler
    : IRequestHandler<VerifiedActiveStudentUnsubscribeFromActiveJobOfferWithVerifiedActiveCompanyCommand>
{
    private readonly IApplicationDbContext _context;

    public VerifiedActiveStudentUnsubscribeFromActiveJobOfferWithVerifiedActiveCompanyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        VerifiedActiveStudentUnsubscribeFromActiveJobOfferWithVerifiedActiveCompanyCommand request,
        CancellationToken cancellationToken)
    {
        var student = await _context.Students
            .Filter(
                isVerified: true
            )
            .FirstOrDefaultAsync(x => x.Id == request.StudentId);

        if (student == null)
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        var jobOffer = await _context.JobOffers
            .Include(x => x.SubscribedStudents)
            .Filter(
                isActive: true,
                isCompanyVerified: true
            )
            .FirstOrDefaultAsync(x => x.Id == request.JobOfferId);

        if (jobOffer == null)
        {
            throw new NotFoundException(nameof(JobOffer), request.JobOfferId);
        }

        var subscribedStudent = jobOffer.SubscribedStudents.FirstOrDefault(x => x.Id == request.StudentId);

        if (subscribedStudent == null)
        {
            return Unit.Value;
        }

        jobOffer.SubscribedStudents.Remove(subscribedStudent);

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}